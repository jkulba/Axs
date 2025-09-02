FROM docker.io/nginx:alpine

COPY nginx/axs-nginx.conf /etc/nginx/conf.d/axs-nginx.conf
COPY dist/wwwroot/ /usr/share/nginx/html/

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]