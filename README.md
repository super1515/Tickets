# Tickets
## Сборка и запуск
Изменить ConnectionString и другие переменные окружения можно в файле *docker-compose.yml*.

Для сборки и запуска проекта необходимо последовательно выполнить следующие команды:
```
docker-compose build
docker-compose up
```
В результате WebAPI будет доступен по адресу *https://localhost:5001*.

В случае если потребуется получить доступ к базе данных извне (например для администрирования в pgAdmin), для подключения можно использовать следующие данные:
* Address: *localhost*
* Port: *5433*
* Username: *postgres*
* Password: *admin*
