class GamesTable {

    // Table
    table = undefined;

    // Search
    searchInput = undefined;

    // Paging
    pageSize = 5;
    pageCurrent = 0;
    pageOverstep = 0;
    pageMax = 1;
    pagingLeft = undefined;
    pagingRight = undefined;
    pagingIndicator = undefined;

    // Events
    onFill = [];
    onClick = [];

    // Objects
    apiWrapper = undefined;

    // Filter
    filtern0 = undefined;
    filtern100 = undefined;
    filterDropped = undefined;
    headerSort = [0, 0]

    //selected data reference
    selectedData = [];

    data = [];
    clearData = [];

    constructor(config) {
        this.table = config.table;
        this.table.rows[0].addEventListener('click', event => {
            this.sortHeader(event);
            this.setHeader();
            this.prepareTable();
        });
        this.searchInput = config.search.input;
        this.searchInput.addEventListener('input', this.prepareTable.bind(this));
        this.pageSize = config.pageSize;
        this.filtern0 = config.filter.n0;
        this.filtern0.addEventListener('change', this.prepareTable.bind(this));
        this.filtern100 = config.filter.n100;
        this.filtern100.addEventListener('change', this.prepareTable.bind(this));
        this.filterDropped = config.filter.dropped;
        this.filterDropped.addEventListener('change', this.prepareTable.bind(this));
        this.apiWrapper = config.apiWrapper;
        this.pagingLeft = config.paging.left;
        this.pagingLeft.onclick = this.pageDown.bind(this);
        this.pagingRight = config.paging.right;
        this.pagingRight.onclick = this.pageUp.bind(this);
        this.pagingIndicator = config.paging.indicator;
        this.selectedData = config.selectedData;
    }

    registerOnFillEvent(callback) {
        this.onFill.push(callback);
        return this;
    }

    registerOnClickEvent(callback) {
        this.onClick.push(callback);
        return this;
    }

    fill() {
        this.apiWrapper.getTableData()
            .then(rows => {
                this.clearData = rows;
            }).then(this.prepareTable.bind(this));

        return this;
    }

    prepareTable() {
        this.searchUpdate();
        this.calculatePaging();
        this.loadPage();
        this.updatedSelected();
        this.callbackFill();
    }

    calculatePaging() {
        this.pageCurrent = 0;
        this.pageOverstep = this.data.length % this.pageSize;
        this.pageMax = (this.data.length - this.pageOverstep) / this.pageSize;
        if (this.pageOverstep !== 0)
            this.pageMax++;
    }

    loadPage() {
        let table = this.table;

        for (let i = table.rows.length - 1; i > 0; i--) {
            table.deleteRow(i);
        }

        let max = this.pageCurrent * this.pageSize + this.pageSize;
        for (let i = this.pageCurrent * this.pageSize; i < max; i++) {
            let rowData = this.data[i];
            if (rowData === undefined)
                rowData = {
                    appid: -i,
                    name: '...',
                    completion: '...',
                    dropped: false,
                }

            let row = table.insertRow(-1);
            row.onclick = event => {
                this.callbackClick(event.target.parentNode, event.ctrlKey);
            }

            let cellAppId = row.insertCell(0);
            cellAppId.textContent = rowData.appid;
            cellAppId.style = 'display: none';

            let cellName = row.insertCell(1);
            cellName.textContent = rowData.name;

            let cellPercent = row.insertCell(2);
            cellPercent.textContent = rowData.percent;

            let cellDropped = row.insertCell(3);
            cellDropped.textContent = rowData.dropped;
            cellDropped.style = 'display: none';
        }
        this.pagingIndicator.textContent = this.pageCurrent + 1;
    }

    pageUp() {
        this.pageCurrent++;
        if (this.pageCurrent >= this.pageMax)
            this.pageCurrent = this.pageMax - 1;
        this.loadPage();
        this.updatedSelected();
    }

    pageDown() {
        this.pageCurrent--;
        if (this.pageCurrent < 0)
            this.pageCurrent = 0;
        this.loadPage();
        this.updatedSelected();
    }

    callbackFill() {
        this.onFill.forEach(item => {
            item(this);
        });
    }

    callbackClick(row, ctrl) {
        this.onClick.forEach(item => {
            item(row, ctrl);
        });
    }

    clickFirstRow() {
        this.table.rows[1].cells[1].click();
    }

    setSelected(selected) {
        this.selectedData = selected;
    }

    updatedSelected() {
        for (let i = this.table.rows.length - 1; i > 0; i--) {
            const row = this.table.rows[i];
            let appid = +row.cells[0].innerText;
            let index = this.selectedData.findIndex(item => +item.appid === appid);
            if (index !== -1)
                row.classList.add('highlight');
            else
                row.classList.remove('highlight');
        }
    }

    searchUpdate(search) {
        let searchValue = this.searchInput.value;
        if (search)
            searchValue = search;

        let drop = this.filterDropped.checked;
        let n0 = this.filtern0.checked;
        let n100 = this.filtern100.checked;

        // there might be some redundant work here!
        let newData = []
        let data = JSON.parse(JSON.stringify(this.clearData));

        data.forEach(obj => {
            let include = true;
            if (drop && !obj.dropped)
                include = false;

            if (n100 && obj.percent === 100)
                include = false;

            if (n0 && obj.percent === 0)
                include = false;

            if (!obj.name.toLowerCase().includes(searchValue.toLowerCase()))
                include = false;

            if (include)
                newData.push(obj);
        });

        const sort = this.headerSort[0];
        const dir = this.headerSort[1];
        newData = newData.sort((a, b) => {


            if (sort === 0)
                return 0;

            let val1;
            let val2;
            if (sort === 1) {
                val1 = a.name;
                val2 = b.name;
            }
            if (sort === 2) {
                val1 = +a.percent;
                val2 = +b.percent;
            }

            if (val1 > val2) return (dir - 2);
            if (val1 < val2) return -(dir - 2);

            return 0;
        });


        this.data = newData;
    }

    sortHeader(event) {
        let sort = 0;
        if (event.target.id === 'name')
            sort = 1;
        if (event.target.id === 'comp')
            sort = 2;

        if (this.headerSort[1] >= 2 && this.headerSort[0] === sort) {
            this.headerSort = [0, -1];
            return;
        }

        if (this.headerSort[0] === sort) {
            this.headerSort[1] += 2;
        } else {
            this.headerSort = [sort, 1];
        }
    }

    setHeader() {
        let sort = this.headerSort[0];
        let dir = this.headerSort[1];
        const cells = this.table.rows[0].cells;
        cells[1].innerText = 'Name';
        cells[2].innerText = 'Completion';
        if (sort === 0)
            return;

        let arrow = '↑';

        if (dir === 3)
            arrow = '↓';

        cells[sort].innerText += arrow;
    }
}