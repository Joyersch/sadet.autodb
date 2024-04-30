class ApiWrapper {
    async doGetRequest(url) {
        try {
            const response = await fetch(url, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                },
            });

            if (response.status === 200) {
                return await response.json();
            } else {
                throw new Error(`Error: Api return ${response.status}`);
            }
        } catch (error) {
            console.error(error);
            console.error(url);
            throw error;
        }
    }

    async getGames() {
        return await this.doGetRequest('api/games');
    }

    async getGameData(id) {
        return await this.doGetRequest('api/data/' + id);
    }

    /*
    * Supported limit values:
    *  week, month, year, alltime
    * */
    async getGameDataWithDateLimit(id, limit) {
        return await this.doGetRequest('api/data/' + id + '/' + limit);
    }

    async getGameDataLimit(id, limit) {
        let data = await this.doGetRequest('api/data/' + id);
        return {
            count: limit,
            data: data.data.splice(-limit)
        };
    }

    async getTableData() {
        return await this.doGetRequest('api/dashboard');
    }
}