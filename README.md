Условие задачи в https://github.com/alex-tselikovsky/EquipmentSupplyHandler/blob/master/task.md

Описание решения EquipmentSupplyHandler (ESH)

Проект ESHRepository.Core

Модели и интерфейсы репозиториев собраны в проекте ESHRepository.Core.
Для перехода на другую технологию хранения данных, необходимо реализовать все интерфейсы репозиториев в данном проекте.
Там же находится интерфейс аналитики. Возможно его стоит вынести в отдельный проект, т.к. если данных будет очень много, скорее всего для аналитики будет необходимо создавать отдельное денормализованное хранилище.
Раздумывал над упрощенной реализацией этого в той же базе с заполнением "на лету" в отдельном потоке/транзакции с помощью, например, сервис брокера и очереди сообщений. 
Необходимо реализовать: протокол возможных исключений. Например, реакция на неверную валидацию, вставку существующего элемента, удаления не существующего и т.д.
В реализациях не должно быть исключений связанных с этими реализациями. Все исключения реализации должны быть перехвачены и преобразованы в исключения согласно протоколу. 

ESHRepository.EF

Реализация хранилища данных с помощью EntityFramworkCore
Так как CRUD в проекте стандарнтый, не видел смысла писать много кода. Реализован обобщенный репозиторий для всех сущностей. В репозитории для поставок добавлен метод извлечения за период.
Аналитика реализована в виде плоских SQL запросов.

Notifications

Реализация потокобезопасных нотификаторов. Потокобезопасность реализована на уровне экземпляров.
ConcurrentDelayedNotificator - собирает сообщения в разных потоках. После поступления первого сообщения запускает таймер, продолжая собирать в очередь сообщения. 
После истечения времени обрабатывает (внедренным обработчиком) порцию полученных сообщений и  запускает новый таймер, если в очередь продолжают поступать сообщения. Реализован неблокирующий доступ к методу нотификации.
ConcurrentDictNotificator - разделяет полученные сообщения по разным индивидуальным нотификаторам согласно идентификаторам сообщений. В качестве индивидуальных нотификаторов в веб приложении используютя экземпляры ConcurrentDelayedNotificator

EquipmentSupplyHandler

Основное веб приложение на базе ASP.Net Core
Реализации репозиториев, нотификаторов и обработчиков событий настраиваются при помощи встроенного IOC контейнера. 
Синглтон типа ConcurrentDictNotificator собирает сообщения в контроллере DeliveryController.
В качестве обработчика событий выступает класс SupplyOperationNotificationProcessor. Объекты класса не хранят состояние, поэтому можно использовать его как синглтон. В процессе обработки сообщения агрегируются и ввиде одного сообщения передаются объекту, реализующему INotificationSender (реализация EmailNotificationSender).

Минус данной реализации заключается в том, что работа веб приложения может быть завершена из-за перезапуска пула приложений. В этом случае необработанные сообщения будут утеряны.
Для решения данной проблемы необходимо использовать внешнее хранилище сообщений, из которого можно восстанавливать не обработанные сообщения. В качестве такого хранилища предлагается использовать шину сообщений, например, RabbitMQ. Его концепция очередей отлично вписывается для использования реализованного нотификатора. Он будет служить обработчиком поступающих сообщений. Подтверждения обработки сообщений (Acknowledgements) будут отправляться только в случае обработки/отправки соответствующего агрегата сообщений. 
В случае использования данного решения нотификатор можно встроить в тоже самое веб приложение с помощью служб размещения. Для этого необходимо реализовать в нотификаторе поддержку отмены операции, отменяющей обработку полученных сообщений.  Эта функциональность позволит в случае штатной перезагрузки пула приложений, корректно завершить работу обработчиков, максимально однозначно определив статус каждого сообщения.

На данный момент в проекте реализована скудная обработка ошибок.

Интеграционные тесты

Выполняются на тестовой базе MSSQL. Выполняются последовательно один за другим, т.к. база одна. 
Необходимо сделать больше проверок на ошибочные данные или не валидные данные.
Тестирование системы нотификаций производится с помощью модульных тестов. Замокал сендер классом собирающем отсылаемые сообщения, но на данный момент не получилось получить один и тот же его экземпляр на сервере и на клиенте-тестировщике при интеграционном тестировании.
Отдельно проверка нотификатора с задержкой производится в тесте  Notification.Infrastructure.Tests.NotificationInfrastructureTest.Test1 (тест выполняется довольно продолжительное время для верности).

