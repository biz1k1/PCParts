# PCParts
Чтобы запустить приложение локально, нужно установить docker for desktop. После этого нужно выполнить команду в директории репозитория
```
docker-compose -f solution-item/docker/docker-compose.yml up
```
Отдельно запускается rabbitmq, он нужен для корректной работы регистрации и верификации
```
docker-compose -f solution-item/rabbitmq/docker-compose.yml up
```
Это поднимет нужные для работы сервисы. Миграции автоматически применяются при запуске приложения.\
После этого нужно перейти по ссылке
```
https://localhost:8081/swagger/index.html
```

Для работы с кодом приложения понадобится установить dotnet 8, рантайм и SDK.
