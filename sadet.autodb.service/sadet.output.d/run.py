import subprocess
import psycopg2
import datetime

process = subprocess.Popen(['sadet', '-ac', '[STEAM_API_KEY]', '76561198350892105', '-lag', '-le', 'ex_games.json', '-pca', '-f', '{2}={1}={0}', '-pc'], stdout=subprocess.PIPE, stderr=subprocess.PIPE)

stdout, tderr =  process.communicate()

sadet = stdout.decode('utf-8').split('\n')

firstLine = True

queries = []

log = open("log.log", "a")

for line in sadet:

    log.write("[{}] Loading -> ".format(datetime.datetime.now()) + line + "\n")
    data = line.split("=")

    if firstLine == True:
        queries.append(f"INSERT INTO data (AppId, Completion, IsAverage) VALUES (-1, {data[1]}, true)");
        firstLine=False
        continue
    
    if sadet[len(sadet) - 1] == line:
            continue # this removes the last line as it will be empty


    if len(data) < 3:
        exit(501) #if sadet returned an error message this will exit

    id = data[0]
    name = data[1].replace("'","''")
    comp = data[2].rstrip()
    queries.append(f"INSERT INTO games VALUES ({id}, '{name}') ON CONFLICT (appid) DO UPDATE SET name='{name}'")
    queries.append(f"INSERT INTO data (AppId, Completion, IsAverage) VALUES ({id}, {comp}, false)")

con = psycopg2.connect(
    host = "localhost",
    database = "sadet",
    user = "pi",
    password = "[PASSWORD]")

cursor = con.cursor()

for query in queries:
    log.write("[{0}] Executing -> ".format(datetime.datetime.now()) + query + "\n")
    cursor.execute(query)

con.commit()

cursor.close()

con.close()
