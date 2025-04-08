### Steps to enter mongodb shell via Putty (VPS)

1. ``` docker exec -it mongo mongosh -u yourUsername -p yourPassword ```
2. ``` show dbs ```
3. ``` db.TenantCollection.find(); ```

### Export backup via docker
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
