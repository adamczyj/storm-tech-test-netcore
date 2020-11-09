var TodoItems = ((todoItems) => {
    var todoItemsListQuery = {
        hideDone: false,
        orderBy: todoItems.TodoListItemContext.orderByImportance
    }

    todoItems.showAll = () => {
        todoItemsListQuery.hideDone = false;
        loadItems();
    }

    todoItems.hideDone = () => {
        todoItemsListQuery.hideDone = true;
        loadItems();
    }

    todoItems.orderByRank = () => {
        todoItemsListQuery.orderBy = todoItems.TodoListItemContext.orderByRank;
        loadItems();
    }

    todoItems.orderByImportance = () => {
        todoItemsListQuery.orderBy = todoItems.TodoListItemContext.orderByImportance;
        loadItems();
    }

    todoItems.updateRank = (todoItemId) => {
        const rank = $(`#item-${todoItemId} input[name="Rank"]`).val();

        $.get({
            type: 'PATCH',
            url: `/api/todoitem/${todoItemId}`,
            data: JSON.stringify({ rank: rank }),
            contentType: 'application/json',
            processData: false
        }).done(() => loadItems());
    }

    function loadItems() {
        $(".todo-items").remove();

        return $.get({
            type: 'GET',
            url: `/api/todolist/${todoItems.TodoListItemContext.todoListId}/items`,
            data: todoItemsListQuery,
            contentType: 'application/json'
        }).done((response) => {
            response.forEach((item) => createRow(item));
        });
    }

    function createRow(item) {
        let gravatarUrl = "https://www.gravatar.com/avatar/";

        let rowObject = {
            todoItemId: item.todoItemId,
            displayName: item.responsibleParty.displayName,
            titleElem: getTitleElem(item),
            imgUrl: gravatarUrl + `${item.hash}?s=30`,
            importanceClass: getImportanceClass(item.importance),
            rank: item.rank
        };

        $("#grid").append($(template(rowObject)));
    }

    function getImportanceClass(importance) {
        if (importance === todoItems.TodoListItemContext.importanceHigh) return "list-group-item-danger";
        if (importance === todoItems.TodoListItemContext.importanceLow) return "list-group-item-info";
        return "";
    }

    function getTitleElem(item) {
        if (item.isDone) return `<s>${item.title}</s>`;
        return `<text>${item.title}</text>`;
    }

    var template =
        ({ todoItemId, titleElem, displayName, imgUrl, importanceClass, rank }) =>
            `<li class="todo-items list-group-item ${importanceClass}" id="item-${todoItemId}">
                    <div class="row">
                        <div class="col-md-4">
                            <a href="/TodoItem/Edit?todoItemId=${todoItemId}">
                                ${titleElem}
                            </a>
                        </div>

                        <div class="col-md-4 text-right">
                            <small>
                                ${displayName}
                                <img src="${imgUrl}" />
                            </small>
                        </div>

                        <div class="col-md-4">
                            <div class="input-group">
                                <input type="number" class="form-control" name="Rank" value="${rank}">
                                <span class="input-group-btn">
                                    <button class="btn btn-default" type="button" onclick="TodoItems.updateRank(${todoItemId})">Set rank</button>
                                </span>
                            </div>
                        </div>
                    </div>
                </li>`;

    todoItems.loadItems = loadItems;

    return todoItems;
})(TodoItems || {});



