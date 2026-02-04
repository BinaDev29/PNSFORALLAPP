# Build stage
FROM node:20-alpine AS build
WORKDIR /app

# Copy package files
COPY package*.json ./
RUN npm install

# Copy source and build
COPY . .
RUN npm run build

# Final stage - serve with Nginx
FROM nginx:stable-alpine AS final
COPY --from=build /app/dist /usr/share/nginx/html
# Copy custom nginx config if needed, or use default
# COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
