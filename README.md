# PCParts

Это приложение для управления комплектующими ПК, их категориями и спецификациями.  

---

## Локальный запуск

1. Установите [Docker for Desktop](https://www.docker.com/products/docker-desktop).  
2. Перед запуском через Docker переименуйте файл `.env.example` в `backend.env` и при необходимости отредактируйте 
переменные.
```
PCParts\solution-item\env
```
3. Перейдите в директорию проекта и выполните команду:

```
docker-compose -f solution-item/docker/docker-compose.yml up
```
Это поднимет нужные для работы сервисы. Миграции автоматически применяются при запуске приложения.\
После этого можно перейти по ссылке
```
https://localhost:8081/swagger/index.html
```

## Краткая информация по проекту
### Создание моделей
Общая логика выглядит так: \
`Category → Specification → SpecificationValue ← Component ← Category`

Краткое описание каждой сущности

1. Category – создаём категорию компонентов.
2. Specification – создаём спецификации для категории.
3. Component – создаём компонент в категории.
4. SpecificationValue – добавляем значения спецификаций к компоненту.

### Логи и метрики


- **Loki** – собирает логи приложения.  
- **Promtail** – собирает логи из Docker-контейнеров.  
- **Prometheus** – собирает метрики приложения.  

Дашборды доступны по ссылке:
```
http://localhost:3000/dashboards
```

### Сервис уведомлений
При регистрации создаётся PendingUser и соответствующее событие в базе (Outbox). BackgroundService Publisher отправляет событие в RabbitMQ, после чего Consumer получает его, вызывает API и отправляет пользователю код верификации.\
\
Общая логика выглядит так: :\
`PendingUser → Outbox → Publisher → RabbitMQ → Consumer → API → Email → Пользователь`
### Кеш
Кеширование GET запросов через  **Redis**.
