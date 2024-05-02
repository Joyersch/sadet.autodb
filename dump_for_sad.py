# this file was mostly created by ChatGPT 4
import psycopg2
import json
from datetime import datetime
from fractions import Fraction

def connect_to_db():
    conn = psycopg2.connect(
        dbname="sadet",
        user="pi",
        password="pi",
        host="localhost",
    )
    return conn

def fetch_data(conn):
    cursor = conn.cursor()
    cursor.execute("SELECT * FROM Games WHERE appid > 0 ORDER BY appid;")
    games = cursor.fetchall()
    cursor.execute("SELECT * FROM Data ORDER BY appid ASC, createdat ASC;")
    data = cursor.fetchall()
    cursor.close()
    return games, data

def transform_data(games, data):
    games_json = [{"id": idx, "created_at": datetime.now().isoformat() + 'Z', "updated_at": datetime.now().isoformat() + 'Z', "appid": game[0], "name": game[1]} for idx, game in enumerate(games)]

    # add you're user name and id (name can be anything as long as it is a value) here
    users_json = [{"id": 1, "name": "Joy", "steam_user_id": 76561198350892105, "created_at": datetime.now().isoformat() + 'Z', "updated_at": datetime.now().isoformat() + 'Z', "game_stats": []}]

    game_ids = {game[0]: idx for idx, game in enumerate(games)}
    current_appid = -1337
    current_value = -10;
    id = 0

    for d in data:
        d_id = d[2]
        d_value = d[3]
        d_dat = d[1]

        if d_id not in game_ids:
            continue

        if current_appid == -1337:
            fraction = Fraction(d_value / 100).limit_denominator()
            users_json[0]["game_stats"].append({
                "id": id,
                "created_at": d_dat.isoformat() + 'Z',
                "updated_at": d_dat.isoformat() + 'Z',
                "user_id": 1,
                "game_id": game_ids[d[2]],
                "total": fraction.denominator,
                "achieved": fraction.numerator
            })
            current_appid = d_id
            current_value = d_value

        if current_appid == d_id and d_value == current_value:
            users_json[0]["game_stats"][-1]['updated_at'] = d_dat.isoformat() + 'Z'
        else:
            if current_appid != d_id:
                id += 1
            fraction = Fraction(d_value / 100).limit_denominator()
            users_json[0]["game_stats"].append({
                "id": id,
                    "created_at": d_dat.isoformat() + 'Z',
                    "updated_at": d_dat.isoformat() + 'Z',
                "user_id": 1,
                "game_id": game_ids[d[2]],
                "total": fraction.denominator,
                "achieved": fraction.numerator
            })
            current_appid = d_id
            current_value = d_value

    activity_json = []

    return {
        "games": games_json,
        "users": users_json,
        "activity": activity_json
    }

def main():
    conn = connect_to_db()
    games, data = fetch_data(conn)
    json_data = transform_data(games, data)

    with open('exported_data.json', 'w') as f:
        json.dump(json_data, f, indent=4)

    print("Files exported successfully!")

if __name__ == '__main__':
    main()
