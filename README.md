## Демонстрационный проект — CrashTracker Demo

### Описание  

**CrashTracker Demo** — API для управления инцидентами. Позволяет создавать, редактировать и отслеживать записи об аварийных ситуациях. Реализована поддержка пользователей с аутентификацией по **JWT-токенам**.  

### Основные функции  

#### Работа с авариями (Crash)  
- Создание, редактирование, удаление, просмотр записей.  
- Отслеживание статусов и автоматический расчет прогресса.  

#### Работа с операциями (Operation)  
- Добавление, редактирование, удаление операций, связанных с устранением аварий.  

#### Управление пользователями (User)  
- Регистрация, аутентификация, авторизация.
- Просмотр списка пользователей.
- Получение записей об авариях по идентификатору пользователя.

### Технологии  

- **C#**, **ASP.NET Core**, **Entity Framework Core**  
- **PostgreSQL**, **Redis**  
- **JWT (JSON Web Tokens)**  
- **CSharpFunctionalExtensions**
- **MediatR**
