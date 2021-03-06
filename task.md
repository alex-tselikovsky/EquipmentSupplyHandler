
Крупной компании требуется веб-сервис для обработки поставок оборудования. Предполагается, что с этим сервисом одновременно работает несколько внутренних информационных систем компании.

Сущности системы:

Поставщик:

* Наименование

* Телефон

* Адрес

* Электронная почта

Тип оборудования:

* Наименование

Поставка:

* Поставщик

* Тип оборудования

* Количество поставленных единиц оборудования

* Дата поставки

Необходим следующий функционал:

Работа со справочниками:

* Работа со справочником поставщиков (Создание, Удаление, Редактирование, Получение полного списка)

* Работа со справочником оборудования (Создание, Удаление, Редактирование, Получение полного списка)

Работа с поставками:

* Создание поставки оборудования на определенную дату с указанием поставщика, типа оборудования и его количества.

* Редактирование поставки (все поля)

* Удаление одной поставки

* Получение поставок за определенный промежуток времени

Аналитика:

* Показать общее количество товаров, ранжированное от большего к меньшему, по типам оборудования, поставленного заданным поставщиком в текущем месяце. Формат вывода: Тип оборудования, Количество.

* Вывести список поставщиков, ранжированный от большего к меньшему по процентному соотношению количества единиц поставленного оборудования за текущий календарный год. Формат вывода: Поставщик, Процент.

Уведомления:

* Для мониторинга процесса поставок менеджерами в сервис необходимо встроить систему уведомлений. При создании или удалении поставки должно генерироваться уведомление с текстовым описанием произошедшего события (Операция, Поставка). На первом этапе предлагается создать транспорт уведомлений на основе email (один адрес, задается в конфигурационном файле).

* Необходимо предусмотреть настройку (в конфигурационном файле) — частота уведомлений (в секундах). Если за этот интервал произошло более одного события по

конкретной поставке (к примеру, создание и несколько событий редактирования), то информация о них должна быть отправлена в одном уведомлении.

Требования к архитектуре приложения:

1. Использование ASP.NET Сore

2. JSON как формат передачи данных.

3. Технология хранения данных на выбор кандидата (с обоснованием).

4. Предусмотреть возможность замены хранилища данных на другую технологию, с минимальными затратами на рефакторинг системы.

5. Предусмотреть возможность замены транспорта уведомлений на другую технологию, к примеру Telegram, с минимальными затратами на рефакторинг системы.

6. Уведомление должно быть гарантированно отправлено, если операция с поставкой прошла успешно.

7. Данные о поставках могут генерироваться с высокой интенсивностью, что не должно спровоцировать лавинообразный рост нагрузки на подсистему уведомлений.

Требования к выполнению задания:

1. Спроектируйте и реализуйте предложенный функционал с использованием языка С# и Microsoft Visual Studio 2017, максимально использовав накопленный вами опыт и современные принципы построения информационных систем.

2. Наполните хранилище данных достаточным для проверки результатов объемом информации.

3. Снабдите проект интеграционными тестами, демонстрирующими работу реализованного функционала.
