# TeamOpossums. Наш проект і як його запустити

Сайт : https://opossums-random-gallery.herokuapp.com/index.html

## База даних

* база даних PostgreSQL.

* розгорнута на сайті heroku і має таку структуру:
https://imgur.com/a/KBmZuee

Faces - містить записи  з про емоцій у кількісному співвідношенні на обличчі з фото
Photo - записує фото за тегом та з заданої папки.


SQL- скрипт для створення таблиць в базі даних
```SQL
CREATE TABLE public.faces
(
    id integer NOT NULL DEFAULT nextval('faces_id_seq'::regclass),
    sadness double precision,
    neutral double precision,
    disgust double precision,
    anger double precision,
    surprise double precision,
    fear double precision,
    happiness double precision,
    photo_id integer,
    CONSTRAINT faces_pkey PRIMARY KEY (id),
    CONSTRAINT photo_face_fkey FOREIGN KEY (photo_id)
        REFERENCES public.photo (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;


CREATE TABLE public.photo
(
    id integer NOT NULL DEFAULT nextval('photo_id_seq'::regclass),
    flickr_id character varying(20) COLLATE pg_catalog."default",
    title character varying(100) COLLATE pg_catalog."default",
    url character varying(100) COLLATE pg_catalog."default",
    is_face boolean,
    CONSTRAINT photo_pkey PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
```

## 1. Flickr API
* заповнює  нову базу даних унікальними фотографіями за допомогою Python скрипта (розташований у папці script)
* як запустити?

(краще поміняти конфіги до бд або очистити нашу бд, налаштування до якої в скрипті)

```console
$ cd script
$ source venv/bin/activate
$ python get_photo_from.py

```

## 2-3-4.Face ++
* бекенд написаний на С# у вигляді API
* приклад запиту до API:
https://opossum-gallery.herokuapp.com/api/PhotosPagination/GetAllPhotos?pageNumber=7&maxRequired=50
* фронтенд JQuery

### Алгоритм для розгортання на heroku cайту 
Крок 0. Створити додаток на Heroku. У нас назва opossum-gallery

Крок 1. Створюємо реліз. 
Команда в консолі Windows в корені проекту: 
```console
dotnet publish -c Release
```
  

Крок 2. Відкриваємо консоль DOСKER.  
   Виконуємо вхід у heroku
```console 
 docker ps
 heroku container:login
 ```
  
Крок 3. Далі скопіювати dockerfile в publish папку
```console 
docker build -t opossum-gallery C:\\Projects\\opossums-app\\OpossumsTestApplication\\bin\\Release\\netcoreapp2.2\\publish
 ```
Крок 4.  
```console 
docker tag opossum-gallery registry.heroku.com/opossum-gallery/web
 ```
Крок 5. 
 ```
docker push registry.heroku.com/opossum-gallery/web
 ```
Крок 6. 
 ```
heroku container:release web  --app opossum-gallery
 ```


Сайт : https://opossums-random-gallery.herokuapp.com/index.html

Віримо в найкраще
Заливаємо в останні хвилини до дедлайну  😀😀😀



### хто що робив?

(у нас дуже різний стек технологій тому і таке різноманіття технологій у тестовому завданні)

@heletrix
- фронтенд 
- розгортання додатку на heroku

@wonkat
- api на ASP.NET 

@lizaviet
- cкрипт
- розгортання бд
