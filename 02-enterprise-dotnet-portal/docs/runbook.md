# Production runbook

- **Health:** `GET /health` — DB check included. If it fails, check logs before restarting.
- **Logs:** correlate by `X-Request-Id` (every response, every log line).
- **Bad deploy:** roll back to the previous Azure DevOps release. Do not hot-patch production.
- **Database:** check the migration applied, then connection string / firewall. Never edit production data by hand without a ticket.
- **Slow query:** check the SQL Server index on Status / CreatedAt, then the pagination cap (100/page), then the query plan — in that order.
- **Concurrency error:** a second worker saved the same case first. Re-read, re-apply, re-save. Do not retry blindly.
