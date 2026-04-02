# Bot Takeover State Machine

OfflineObserved -> GracePeriod -> BotWarmup -> Controlling -> HandbackPending -> HumanControlled

## Zdarzenia
- heartbeat timeout,
- reconnect,
- manual enable bot,
- manual disable bot,
- slot force-close.
