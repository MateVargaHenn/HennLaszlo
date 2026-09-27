# Private preview on the existing VPS

The public `hennlaszlo.hu` virtual host remains untouched. The Compose gateway
binds only `127.0.0.1:18080`; PostgreSQL and the application ports have no
public host binding. RabbitMQ management (`15672`) and Seq (`15341`) also bind
only to loopback. RabbitMQ is started for the planned messaging integration;
the current API does not yet publish or consume messages.

## One-time VPS preparation

1. Point `preview.hennlaszlo.hu` at the VPS. Create a separate TLS certificate
   and install `deploy/host-nginx.conf.example` as a new Nginx virtual host.
   Confirm `nginx -t` before reloading. Do not alter the existing domain.
2. Clone this repository at `/var/www/hennlaszlo-preview` on the VPS. Copy
   `.env.example` to `.env` and set unique nonempty PostgreSQL, RabbitMQ,
   Seq and admin values. Set `PREVIEW_HOST` to the exact TLS hostname. Keep
   `.env` out of Git and restrict it to the deploy user (`chmod 600 .env`).
3. Generate `deploy/preview.htpasswd` with a strong preview password, e.g.
   `htpasswd -cB deploy/preview.htpasswd matyi`, and restrict file access.
   This outer password protects the whole site, including its API and admin UI.
   The admin UI has its own account as well.
4. To generate `ADMIN_PASSWORD_HASH`, first give it a temporary nonempty value
   in `.env`, build the API (`docker compose build backend`) and run
   `docker compose run --rm --no-deps backend --hash-admin-password`. Paste
   the resulting ASP.NET Identity hash into `.env`, then remove the temporary
   value. Do not commit the hash or passwords.
5. Back up the PostgreSQL and file-storage volumes before future updates.
   The API applies its four EF Core migrations during startup. Use a single
   API instance while migrations run.
6. In GitHub's `private-preview` environment set secrets `VPS_HOST`,
   `VPS_USER`, `VPS_SSH_KEY`, `VPS_KNOWN_HOSTS`, `PREVIEW_USER`, and
   `PREVIEW_PASSWORD`. Pin the actual SSH host key in `VPS_KNOWN_HOSTS` after
   verifying its fingerprint out of band. Set environment variable
   `PREVIEW_HOST=preview.hennlaszlo.hu`. The `PREVIEW_USER` and password must
   match `deploy/preview.htpasswd`.

The **Private preview** workflow validates Angular and .NET, builds all
containers, then checks that anonymous requests receive 401 and authenticated
SSR, API, and admin requests succeed. Run the workflow manually on
`development` to deploy the validated commit; it checks the HTTPS URL too.
Changes pushed to `development` run CI only. The deployment does not publish
or replace the main domain.

Access: `https://preview.hennlaszlo.hu/` (only after DNS, TLS, and the workflow
succeed). Admin: `/admin/`. Seq and RabbitMQ management can be reached only
through an SSH tunnel to their loopback ports.
