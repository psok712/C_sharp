# База данных магазина


БД содержит 4 таблицы:

  ● Shop – оффлайн магазин, оборудованный для продажи товаров. Каждый
    магазин имеет уникальный номер, а также содержит информацию о
    своем названии и месторасположении.
    
  ● Buyer – покупатель – обычный человек, совершающий покупки. Сущность
    покупателя в базе данных содержит информацию о своем уникальном
    идентификаторе, имени, фамилии и месте проживания.
    
  ● Good – товар, лежащий на полке определенного магазина. Каждый товар
    описывается собственным уникальным номером, идентификатором
    магазина, в котором его можно купить, а также категорией, к которой этот
    товар относится и, конечно же, ценой.
    
  ● Sale – совершенная в магазине покупка определенного товара. Покупка
    содержит информацию о покупателе, совершившем эту покупку (его id),
    магазине, в котором покупка была совершена (его id), товаре, который
    был куплен (его id), и количестве этого товара.
    
------------------------------------------------------------------------------------------------------------

Класс DataBaseException содержит реализацию пользовательского
исключения, генерируемого при ошибках во время работы с таблицами БД:

  ● IEntity – этот интерфейс описывает сущность, хранимую в БД. Его
    реализуют все наши модельные сущности.
    
  ● IDataBase – этот интерфейс содержит список обобщенных методов,
    которые позволяют работать с таблицами из БД (создавать и получать
    таблицу, добавлять записи в таблицу, а также сериализовать и
    десериализовать содержимое). Методы этого интерфейса вам
    необходимо реализовать самостоятельно.
    
  ● IDataAccessLayer – данный интерфейс содержит набор методов, которые
    обращаются к БД. Фактически они представляют собой LINQ-запросы,
    которые вам необходимо реализовать самостоятельно.
