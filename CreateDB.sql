create table category (id serial primary key, name varchar(100));
create table pick_up_point (id serial primary key, adress varchar(200), time int);
create table "user" (id serial primary key, email varchar (50), name varchar(100), password varchar(100), code varchar(100), isApp boolean);
create table good (id serial primary key, name varchar(100), description varchar(200), price decimal, categoryId int, image text);
create table buy (id serial primary key, goodId int, userId int, email varchar(100), datecreate timestamp, datedelivery timestamp, isBasket boolean);

