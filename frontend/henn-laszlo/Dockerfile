FROM node:24-alpine AS build
WORKDIR /app
COPY package.json package-lock.json ./
RUN npm ci --ignore-scripts
COPY . .
RUN npm run build:apps

FROM node:24-alpine AS public
WORKDIR /app
ENV NODE_ENV=production PORT=4000
COPY --from=build /app/dist/public-web ./dist/public-web
COPY --from=build /app/node_modules ./node_modules
USER node
EXPOSE 4000
CMD ["node", "dist/public-web/server/server.mjs"]

FROM nginx:1.29-alpine AS admin
COPY --from=build /app/dist/admin-web/browser /usr/share/nginx/html
COPY deploy/admin.nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
