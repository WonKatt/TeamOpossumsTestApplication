from flickrapi import FlickrAPI
from config.config import FLICKR_PUBLIC, FLICKR_SECRET
import psycopg2
from datetime import datetime

# credetianls for postgreSQL bd on heroku 
hostname = 'ec2-79-125-6-250.eu-west-1.compute.amazonaws.com'
username = 'txnxharkyeqvfl'
password = '7f6a0ea2bcff69e8471a1c04b15452120bcdf63fa2d4acaffaf2a216a787d3b1'
database = 'd5h6stb0hfhccq'

def insert_to_db(a):
    myConnection = psycopg2.connect (host=hostname, user=username, password=password, dbname=database)
    cur = myConnection.cursor()
    for i in a:
        query = "INSERT INTO photo(flickr_id, title, url) VALUES (%s, %s, %s);"
        cur.execute(query, (i[0], i[1], i[2]))
    myConnection.commit()
    myConnection.close()

if __name__=='__main__':
    #connect to Flickr
    flickr = FlickrAPI (FLICKR_PUBLIC, FLICKR_SECRET, format='parsed-json')
    extras = 'url_c'
    #request for search by tag
    tags_result = flickr.photos.search(tags='int20h', extras=extras)
    #request for search in folder
    link_result = flickr.photosets.getPhotos(photoset_id='72157674388093532', user_id='144522605@N06',extras=extras)
    a = []
    test = []
    # search for unique photos and collect in list 
    for i in tags_result['photos']['photo']:
        a.append([i['id'], i['title'], i['url_c']])
    for i in link_result['photoset']['photo']:
        if any(i['id'] in it[0] for it in a):
            break
        else:
            a.append([i['id'], i['title'], i['url_c']])
    print("find " + str(len(a)) + " photos\n")
    # insert to db results
    # uncomment this if data base is empty
    insert_to_db(a)
