### Steps to enter mongodb shell via Putty (VPS)

```
docker exec -it mongo mongosh -u yourUsername -p yourPassword
```

```
show dbs
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
