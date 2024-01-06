FROM nginx
COPY default /etc/nginx/conf.d/default.conf
RUN sed -i "s|%1%|%APP_SERVICE_SUBDOMAIN%|g" /etc/nginx/conf.d/default.conf
