# Веб-приложение University.Web

### Старт приложения

*Важно! Сделать миграцию и обновить БД*

### Описание

Приложение создано в учебных целях, реализует функционал CRUD с некоторыми дополнениями. Позволяет создавать курсы, группы и студентов. Просматривать общие списки каждой сущности, помимо этого можно посмотреть списки групп курса, списки студентов в группе. Реализовано быстрое удаление в списке с применением технологии AJAX. Присутствует возможность редактирования и каскадного удаления.

### Технологии

* ASP.NET Core 3.1
* Entity Framework Core 3.1
* jQuery.ajax.unobtrusive
* jQuery.validation
* Bootstrap
* xUnit
* Bogus

### Выполненные задачи

- [x] Создание сущностей классов
    - [x] Сущность Course
    - [x] Сущность Group
    - [x] Сущность Student
- [x] Создание репозиториев для каждой сущности
- [x] Создание контроллеров и экшенов
    - [x] Добавление 
    - [x] Удаление
    - [x] Редактирование
    - [x] Показать все
    - [x] Показать группы/студенты
- [x] Создание View для операций 
    - [x] Добавить
    - [x] Посмотреть все
    - [x] Частичные View для AJAX
- [x] Адаптирование View для AJAX
- [x] Настройка атрибутивной валидации
- [x] Интеграционные тесты
    - [x] Тест контроллера CourseController
    - [x] Тест контроллера GroupController
    - [x] Тест контроллера StudentController

### Тесты

Для приложения написаны интеграционные тесты контроллеров с использованием фреймворка xUnit и библиотеки генерации рандомных данных Bogus
