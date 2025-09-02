FROM docker.io/nginx:alpine

COPY nginx/volcano-nginx.conf /etc/nginx/conf.d/volcano-nginx.conf
COPY dist/wwwroot/ /usr/share/nginx/html/ 

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]