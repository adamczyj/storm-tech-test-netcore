const CreateItem = (() => {
    const defaultResponsiblePartyId = getFieldValue("ResponsiblePartyId");

    function addNewItem(event, todoListId) {
        event.preventDefault();
        var item = createItemDto(todoListId);
        postNewItem(item).done(() => clearForm());
    }

    function postNewItem(item) {
        return $.ajax({
            type: 'POST',
            url: '/api/todoitem',
            data: JSON.stringify(item),
            contentType: 'application/json',
            processData: false
        });
    }

    function createItemDto(todoListId) {
        return {
            TodoListId: todoListId,
            Title: getFieldValue("Title"),
            ResponsiblePartyId: getFieldValue("ResponsiblePartyId"),
            Importance: getFieldValue("Importance")
        }
    }

    function clearForm() {
        $("#Title").val('');
        $("#Importance").val('High');
        $("#ResponsiblePartyId").val(defaultResponsiblePartyId);
    }

    function getFieldValue(id) {
        return $(`#${id}`).val();
    }

    return {
        addNewItem: addNewItem
    };
})();