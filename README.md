# Team: Opossum. Наш проект і як його запустити

**Про проект**: веб-сайт, що відслідковує емоції з хакатону INT20.

**Посилання**: https://opossums-random-gallery.herokuapp.com/index.html

**Стек технологій**:
- Backend: ASP .NET Core, EF Core (C#), Python;
- Frontend: Bootstrap 4.2.1 + jQuery 3.3.1;
- Database: PostgreSQL;
- For deployment: Heroku, Docker.

## База даних: PostgreSQL

- БД складається з двох таблиць:
    - Photo - містить фото з flickr (за тегом та з заданої папки) та ознаку наявності обличчя на фото;
    - Faces - містить максимальні значення емоцій з облич на фото (обмеження face++ - до 5 облич).
- Схема БД: https://imgur.com/a/KBmZuee;
- БД розгорнуто на Heroku. Інструкція з розгортання БД PostgreSQL та налаштування підключення до неї з PgAdmin4: [Getting Started with Heroku, Postgres and PgAdmin](https://medium.com/@vapurrmaid/getting-started-with-heroku-postgres-and-pgadmin-run-on-part-2-90d9499ed8fb).
- Для створення таблиць в БД, запустити SQL-скрипт:
```SQL
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
```

## Завдання 1. Взаємодія з Flickr API
* Python скрипт (розташований у папці script) заповнює нову БД унікальними фотографіями.
* Як запустити?
1. перед запуском скрипта, рекомендуємо поміняти конфіги до БД або очистити БД, налаштування до якої зазначені в скрипті)

```python
# credetianls for postgreSQL bd on heroku 
hostname = [your hostname]
username = [your username]
password = [your password]
database = [your database]

```

2. виконати в консолі наступні комнди:
```console
$ cd script
$ source venv/bin/activate
$ python get_photo_from.py

```

## Завдання 2-4.Face ++
* Backend: API написано на С# (ASP .NET Core + EF Core). Приклад запиту до API:
https://opossum-gallery.herokuapp.com/api/PhotosPagination/GetAllPhotos?pageNumber=7&maxRequired=50 ;
* Frontend: bootstrap 4.2.1 + jquery 3.3.1 .

### Інструкція з розгортання API (проекту ASP .NET Core) на хмарному сервісі Heroku з використанням Docker:

**Крок 0. Prerequisites:**

- [Heroku CLI](https://devcenter.heroku.com/articles/heroku-cli)
- [Docker Toolbox](https://docs.docker.com/toolbox/toolbox_install_windows/)

**Крок 1. Створити додаток на Heroku**
Зареєструватися на Heroku. Зайти у Heroku Dashboard натиснути на кнопку New -> Create new app. Ввести App name, обрати регіон та натиснути на кнопку "Create app". В нас додаток називається "opossum-gallery" та розміщено у регіоні Європа. На вкладці "Deploy" обрати метод розгортання "Container Registry".

**Крок 2.** Зклонувати проект з Github або завантажити архів та розархівувати його на комп'ютері. Якщо потрібно, змінити параметри підключення до бази даних (наприклад, якщо було створено нову БД) у файлі connection.json (Шлях: TeamOpossumsTestApplication\OpossumsTestApplication\wwwroot\jsons\DbConnection).

**Крок 3.** Зайти в корінь папки з проектом та ствоорити файл Dockerfile з наступним вмістом:
```
FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base

WORKDIR /app
COPY . .

CMD ASPNETCORE_URLS=http://*:$PORT dotnet OpossumsTestApplication.dll
```
**Крок 4.** Зкомпілювати проект конольною командою:
```dotnet publish -c Release```
В результаті буде створено папку publish. (Шлях до папки виглядає наступним чином: 'C:\Projects\opossums-app\OpossumsTestApplication\bin\Release\netcoreapp2.2\publish').

**Крок 5.** В щойно створену папку publish скопіювати файл Dockerfile

**Крок 6.** Відкрити консоль Docker та увійти в Container Registry
```heroku container:login```

**Крок 7.** Збілдити docker image командою
```docker build -t opossum-gallery ```

```C:\\Projects\\opossums-app\\OpossumsTestApplication\\bin\\Release\\netcoreapp2.2\\publish```
**Крок 8.** Створити tag
```docker tag opossum-gallery registry.heroku.com/opossum-gallery/web```

**Крок 9.** Зробити push Docker image
```docker push registry.heroku.com/opossum-gallery/web```

**Крок 10.** Завершити розгортання командою:
```heroku container:release web  --app opossum-gallery```

**Крок 11.** Впевнитися, що API розгорнуто:
https://opossum-gallery.herokuapp.com/api/PhotosPagination/GetAllPhotos?pageNumber=7&maxRequired=50 
Якщо виникли помилки, подивитися логи.

**Корисні посилання:**
1. [Deploy Asp.Net Core Website on Heroku using Docker](https://www.youtube.com/watch?v=gQMT4al2Grg&app=desktop)
2. [Hosting ASP.NET Core applications on Heroku using Docker]( https://dotnetthoughts.net/hosting-aspnet-core-applications-on-heroku-using-docker/)


### Інструкція з розгортання frontend частини на хмарному сервісі Heroku:

**Крок 0. Prerequisites:**
[Heroku CLI](https://devcenter.heroku.com/articles/heroku-cli) 

**Крок 1.** У файлі js.js присвоїти змінній host значення адреси розгорнутого екземпляру API: 
```var host = "opossum-gallery.herokuapp.com"; ```

**Крок 2.** Відкрити консоль у папці frontend та створити коміт
```git commit -m "Host was changed."```

**Крок 3.** Увійти до Heroku:
```heroku login```

**Крок 4.** Створити новий додаток:
```heroku apps:create opossums-random-gallery```

**Крок 5.** Розгорнути сайт:
```git push heroku master```

**Крок 6.** Перевірити результат за посиланням: https://opossums-random-gallery.herokuapp.com/index.html

**Корисні посилання:**
1. [Як розгорнути статичний сайт на Heroku](https://blog.teamtreehouse.com/deploy-static-site-heroku)


Віримо в найкраще
Заливаємо в останні хвилини до дедлайну  😀😀😀

### Хто що робив?
(у нас дуже різний стек технологій тому і таке різноманіття технологій у тестовому завданні)

**@heletrix** 
- frontend та його розгортання на Heroku;
- створення бази даних PostgreSQL;
- розгортання API (ASP .NET Core) на Heroku.

**@wonkat**
- взаємодія з Face++ API (ASP .NET Core);
- backend: API (ASP .NET Core).

**@lizaviet**
- взаємодія з Flickr API (Python script);
- розгортання бази даних PostgreSQL на Heroku.
