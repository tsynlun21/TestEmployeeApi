Написано на C#, .NET 6.0, Web Api
База данных - MS SQL


# Документация:



## POST /EmployeesApi/insertEmployee

метод загрузки в базу данных модель сотрудника

возвращает id созданного объекта

## DELETE /EmployeesApi/{id}

метод удаления из базы данных сотрудника по его id

возвращает код 200, если сотрудник был найден в ином случае - сообщение, что сотрудник не найден

## PATCH /EmployeesApi/{id}

метод для обновления данных о сотруднике в базе данных

возвращает код 200, если сотрудник был найден в ином случае - сообщение, что сотрудник не найден

## GET /EmployeesApi/getByCompanyName?name={name}

метод для отображения списка сотрудников, работающих в компании {name}

возвращает 404 и сообщение, если таких сотрудников не найден

## GET /EmployeesApi/getByCompanyName?Name={name}&Phone={phone}

метод для отображения списка сотрудников, работающих в отделе {name} с телефонным номером {phone}

возвращает 404 и сообщение, если таких сотрудников не найден
