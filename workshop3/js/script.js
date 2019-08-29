
var bookDataFromLocalStorage = [];
$(document).ready(function(){
  loadBookData();
    $("#wnd").kendoWindow();   
    $("#bought_datepicker").kendoDatePicker();
    $("#book_grid").kendoGrid({
        dataSource: {
            data: bookDataFromLocalStorage,
            schema: {
                model: {
                    fields: {
                        BookId: {type:"int"},
                        BookName: { type: "string" },
                        BookCategory: { type: "string" },
                        BookAuthor: { type: "string" },
                        BookBoughtDate: { type: "string" }
                    }
                }
            },
            sort: {
                field: "bookId",
                dir: "asc"
            },
            pageSize: 20,
        },
        toolbar: kendo.template("<div class='book-grid-toolbar'><input class='book-grid-search' placeholder='搜尋:書名......' type='text'></input></div>"),
        height: 550,
        sortable: true,
        editable: true,
        pageable: {
            input: true,
            numeric: false
        },
        columns: [
            { field: "BookId", title: "書籍編號",width:"10%"},
            { field: "BookName", title: "書籍名稱", width: "50%" },
            { field: "BookCategory", title: "書籍種類", width: "10%" },
            { field: "BookAuthor", title: "作者", width: "15%" },
            { field: "BookBoughtDate", title: "購買日期", width: "15%" },
            { command: { text: "刪除", click: deleteBook }, title: " ", width: "120px" }
        ]
        
    });
    
    
//field:欄位、operator:限制
    $(".book-grid-search").on("input",function () {
        var val = $(".book-grid-search").val();
        $("#book_grid").data("kendoGrid").dataSource.filter({
            
            filters: [
                {
                    field: "BookName",
                    operator: "contains",
                    value: val
                },
                
            ]
        });
    
    
    });
})

function loadBookData(){
    bookDataFromLocalStorage = JSON.parse(localStorage.getItem("bookData"));
    if(bookDataFromLocalStorage == null){
        bookDataFromLocalStorage = bookData;
        localStorage.setItem("bookData",JSON.stringify(bookDataFromLocalStorage));
    }
   // console.log(bookDataFromLocalStorage);
}
/*設變數CImage，用val()取book_categor裡的項目來選擇對應的圖片*/
function onChange(){
    var CImage= $("#book_category").val()
    $(".book-image").attr("src",CImage)
}
 
function deleteBook(e){   
    e.preventDefault();  //只處理刪除的事件，不會進行重新整理等後續的動作
    var tr = (e.target).closest("tr"); //e.target:事件觸發，closest("tr"):被觸發button的tr(表格內)
    var data1 =this.dataItem(tr);
    var grid = $("#book_grid").data("kendoGrid").dataSource;
          
    grid.remove(data1);
    localStorage.setItem('bookData', JSON.stringify(grid.data())); //更新資料庫裡的資料
}

var wnd = $("#wnd").kendoWindow({
    title:"新增書籍",
    height: 500,
    width: 600,
    visible: false
  }).data("kendoWindow");

$(".k-button").click(function(e) {
    
    wnd.open();
    var data = [
        {text:"資料庫",value:"image/database.jpg"},
        {text:"網際網路",value:"image/internet.jpg"},
        {text:"應用系統整合",value:"image/system.jpg"},
        {text:"家庭保健",value:"image/home.jpg"},
        {text:"語言",value:"image/language.jpg"}
    ]
    $("#book_category").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: data,
        index: 0,
        change: onChange   //可能會有更多個kendoDropDownList，不能這樣設
    });

    $("#abc").click(function(e) {
        //需驗證是否為空值、時間的格式等
        var Bcategory= $("#book_category").data("kendoDropDownList").text();
        var Bname= $("#book_name").val();
        var BAuthor= $("#book_author").val();
        var BBoughtDate=$("#bought_datepicker").val();  

        var grid = $("#book_grid").data("kendoGrid");
        var DSource = grid.dataSource;
        var total=DSource.data();
        var len = (total[(total.length)-1].BookId)+1;   //要取最大的BookId+1
            DSource.insert({BookId:len, BookName:Bname, BookCategory:Bcategory,BookAuthor:BAuthor,BookBoughtDate:BBoughtDate}); 
            alert('New row added !!');
            localStorage.setItem('bookData', JSON.stringify(DSource.data()));
            wnd.close();
    });
});

    