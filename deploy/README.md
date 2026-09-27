# Private IP preview on the shared VPS

The Compose gateway binds only `127.0.0.1:18080`; PostgreSQL, RabbitMQ, Seq,
and application ports have no host binding. The public HTTPS endpoint uses its
own port, `18443`, leaving existing sites on ports 80 and 443 intact. The IP
HTTP virtual host serves ACME challenges only; all other paths return 404.
The preview is protected by HTTP Basic Auth and sends `X-Robots-Tag: noindex`.
An IP address can still be found by scanners; access control is the password.
RabbitMQ is ready for planned messaging integration; the current API does not
yet publish or consume messages.

## One-time VPS preparation

1. Verify that the VPS serves HTTP with Nginx, `18443` and `18080` are free,
   and that no existing Nginx block has `server_name 194.182.85.127`. Keep the
   existing domain configurations unchanged. Clone this repository at
   `/var/www/hennlaszlo-preview`. Install Certbot 5.4 or newer. On this shared
   VPS, keep the existing apt Certbot for other domains and call the new Snap
   version as `/snap/bin/certbot`. Disable only the Snap renewal service with
   `sudo snap stop --disable certbot.renew`; the dedicated timer below handles
   the IP certificate. Do not remove or disable the apt Certbot timer.
2. Copy `.env.example` to `.env` there. Set unique nonempty PostgreSQL,
   RabbitMQ, Seq, and admin values; leave `PREVIEW_HOST=194.182.85.127`.
   Restrict `.env` to the deploy user (`chmod 600 .env`). Generate
   `deploy/preview.htpasswd` using `htpasswd -cm deploy/preview.htpasswd matyi`
   with a unique, long random password (at least 24 characters). The gateway
   uses the `apr1` format tested by CI; make the file readable by the container
   with `chmod 644 deploy/preview.htpasswd`. This protects the whole preview,
   including the API
   and admin UI.
3. Generate the admin hash: give `ADMIN_PASSWORD_HASH` a temporary nonempty
   value, run `docker compose build backend`, then
   `docker compose run --rm --no-deps backend --hash-admin-password`. Put the
   resulting ASP.NET Identity hash in `.env`, then remove the temporary value.
   Do not commit passwords or hashes. The API applies four EF Core migrations
   at startup; use one API instance during migrations. Back up its PostgreSQL
   and file-storage volumes before future updates.
4. Create an ACME challenge root outside the checkout so the Nginx worker can
   read it even though the checkout itself is mode `750`. Install only the
   IP-specific HTTP config:

   ```bash
   sudo install -d -m 755 /var/lib/hennlaszlo-preview-acme/.well-known/acme-challenge
   sudo cp deploy/ip-challenge.nginx.conf.example /etc/nginx/sites-available/hennlaszlo-ip-challenge.conf
   sudo ln -s /etc/nginx/sites-available/hennlaszlo-ip-challenge.conf /etc/nginx/sites-enabled/hennlaszlo-ip-challenge.conf
   sudo nginx -t && sudo systemctl reload nginx
   ```

   Test the challenge path with a temporary file at
   `http://194.182.85.127/.well-known/acme-challenge/<name>` before requesting
   a certificate. Keep the IP certificate and its account separate from all
   existing certificates. First test with `--dry-run`, then repeat without it
   to request the trusted certificate:

   ```bash
   sudo /snap/bin/certbot --config-dir /etc/letsencrypt-hennlaszlo --work-dir /var/lib/letsencrypt-hennlaszlo --logs-dir /var/log/letsencrypt-hennlaszlo certonly --dry-run --preferred-profile shortlived --cert-name hennlaszlo-preview --webroot --webroot-path /var/lib/hennlaszlo-preview-acme --ip-address 194.182.85.127
   sudo /snap/bin/certbot --config-dir /etc/letsencrypt-hennlaszlo --work-dir /var/lib/letsencrypt-hennlaszlo --logs-dir /var/log/letsencrypt-hennlaszlo certonly --preferred-profile shortlived --cert-name hennlaszlo-preview --webroot --webroot-path /var/lib/hennlaszlo-preview-acme --ip-address 194.182.85.127
   ```

5. Install the HTTPS block after the certificate exists:

   ```bash
   sudo cp deploy/host-nginx.conf.example /etc/nginx/sites-available/hennlaszlo-ip-preview.conf
   sudo ln -s /etc/nginx/sites-available/hennlaszlo-ip-preview.conf /etc/nginx/sites-enabled/hennlaszlo-ip-preview.conf
   sudo nginx -t && sudo systemctl reload nginx
   ```

   Allow inbound TCP `18443` in the VPS firewall. The gateway's port `18080`
   remains loopback-only. Confirm the hostname check in the Angular SSR build
   permits the IP host. Install the dedicated renewal timer; the IP certificate
   is short lived and must renew automatically:

   ```bash
   sudo cp deploy/hennlaszlo-preview-certbot.service.example /etc/systemd/system/hennlaszlo-preview-certbot.service
   sudo cp deploy/hennlaszlo-preview-certbot.timer.example /etc/systemd/system/hennlaszlo-preview-certbot.timer
   sudo systemctl daemon-reload
   sudo systemctl enable --now hennlaszlo-preview-certbot.timer
   sudo /snap/bin/certbot --config-dir /etc/letsencrypt-hennlaszlo --work-dir /var/lib/letsencrypt-hennlaszlo --logs-dir /var/log/letsencrypt-hennlaszlo renew --dry-run
   ```

   Check `systemctl list-timers hennlaszlo-preview-certbot.timer` and confirm
   the renewal test succeeds. The service reloads Nginx only after successful
   renewal. The apt Certbot and its existing certificates stay separate.
6. In GitHub's `private-preview` environment, set secrets `VPS_HOST`,
   `VPS_USER`, `VPS_SSH_KEY`, `VPS_KNOWN_HOSTS`, `PREVIEW_USER`, and
   `PREVIEW_PASSWORD`. Verify the SSH host fingerprint out of band.
   Set the environment variable `PREVIEW_URL=https://194.182.85.127:18443`;
   the preview credentials must match `deploy/preview.htpasswd`.

The **Private preview** workflow validates Angular and .NET, builds all
containers, checks anonymous 401 and authenticated SSR, API, and admin
responses. Run it manually on `development` to deploy the validated commit and
verify the live HTTPS URL. Pushes to `development` run CI only.

Access after deployment: `https://194.182.85.127:18443/`. Admin: `/admin/`.
Seq and RabbitMQ management stay on the private Compose network.
