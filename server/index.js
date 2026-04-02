const WebSocket = require('ws');
const { v4: uuidv4 } = require('uuid');

const PORT = process.env.PORT || 3000;
const wss = new WebSocket.Server({ port: PORT });
console.log(`WebSocket server listening on ws://localhost:${PORT}`);

// Simple in-memory rooms map: roomId -> {players: Map(clientId->ws), state: {}}
const rooms = new Map();

function send(ws, obj) {
  ws.send(JSON.stringify(obj));
}

wss.on('connection', (ws) => {
  const clientId = uuidv4();
  ws.clientId = clientId;
  console.log('client connected', clientId);
  send(ws, { type: 'connected', clientId });

  ws.on('message', (message) => {
    let msg;
    try { msg = JSON.parse(message); } catch (e) { return; }

    switch (msg.type) {
      case 'createRoom': {
        const roomId = uuidv4();
        rooms.set(roomId, { players: new Map([[clientId, ws]]), state: {} });
        ws.roomId = roomId;
        send(ws, { type: 'roomCreated', roomId });
        console.log(`room ${roomId} created by ${clientId}`);
        break;
      }
      case 'joinRoom': {
        const { roomId } = msg;
        const room = rooms.get(roomId);
        if (!room) { send(ws, { type: 'error', message: 'room not found' }); break; }
        room.players.set(clientId, ws);
        ws.roomId = roomId;
        // notify existing players
        for (const [id, peer] of room.players) {
          if (id !== clientId) send(peer, { type: 'playerJoined', clientId });
        }
        send(ws, { type: 'joined', roomId });
        console.log(`${clientId} joined room ${roomId}`);
        break;
      }
      case 'startRound': {
        const { roomId } = ws;
        const room = rooms.get(roomId);
        if (!room) { send(ws, { type: 'error', message: 'room not found' }); break; }
        room.state.roundStarted = true;
        // broadcast simple state
        for (const [, peer] of room.players) {
          send(peer, { type: 'roundStarted', roomId });
        }
        console.log(`round started in ${roomId}`);
        break;
      }
      case 'syncState': {
        const { roomId, state } = msg;
        const room = rooms.get(roomId);
        if (!room) break;
        room.state = Object.assign(room.state || {}, state);
        // broadcast to others
        for (const [id, peer] of room.players) {
          if (id !== clientId) send(peer, { type: 'stateUpdate', from: clientId, state });
        }
        break;
      }
    }
  });

  ws.on('close', () => {
    console.log('client disconnected', clientId);
    const roomId = ws.roomId;
    if (roomId) {
      const room = rooms.get(roomId);
      if (room) {
        room.players.delete(clientId);
        for (const [, peer] of room.players) send(peer, { type: 'playerLeft', clientId });
        if (room.players.size === 0) rooms.delete(roomId);
      }
    }
  });
});
