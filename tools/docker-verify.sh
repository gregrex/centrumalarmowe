#!/usr/bin/env bash
set -e
docker compose up -d
docker compose ps
docker compose down -v
