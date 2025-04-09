## Summary
> [!NOTE]  
> Custom implementation of multi-tenant service registry and dynamic service discovery including active health checks with retry mechanisms and fallback strategy, and real-time stream of logs for each service using SSE protocol.

## Technologies
* .NET 8.0
* Multi-tenant application
* REPR Pattern
* Vertical Slice Architecture
* CQRS Pattern
* Mongodb
* Resilience Patterns (retry and fallback)
* Active health check and auto deregister the unhealthy services
* streaming logs via Server-Sent Events (SSE)
* Rate limiting for each tenant with daily rate limit
* Docker for containerization
* Bind mount and volume mount
* NGINX as reverse proxy
* Active-Passive failover
* Weighted Round-Robin load balancing (Layer 7)

### How to Run?

```
git clone https://github.com/Linn-Thit-Htoo/CustomServiceRegistr.git
```

```
docker-compose -f docker-compose-uat.yml -p serviceregistry up -d --build
```

### Steps to enter mongodb shell via Putty (VPS)

```
docker exec -it mongo mongosh -u yourUsername -p yourPassword
```

```
show dbs
```

```
use db_name
```

```
show collections
```

```
db.TenantCollection.find();
```

### Export backup inside the container
```
docker exec mongo \
  mongoexport \
  --username=root \
  --password=root \
  --authenticationDatabase=admin \
  --db=ServiceRegistry \
  --collection=TenantCollection \
  --out=/data/backup/TenantCollection.json
```
### Copy the backup file from the container to the host machine
```
docker cp mongo:/data/backup/TenantCollection.json ./TenantCollection.json
```
