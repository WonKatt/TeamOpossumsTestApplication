# TeamOpossums. Наш проект і як його запустити


## База даних

* база даних PostgreSQL.

* розгорнута на сайті heroku і має таку структуру:




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
* заповнює  нову базу даних унікальними фотографіями за допомогою Python скрипта
скрипт розташований у папці скрипт
* як запустити?

очистіть базу даних!

```console
$ cd script
$ source venv/bin/activate
$ python get_photo_from.py

```
