//Ascending order
var itemsPerPage = 12;
var currentNumberPages = 1;
var currentPage = 1;
var currentFilter;
var filterAttribute = 'data-filter';
var filterValue = "";
var pageAttribute = 'data-page';
var pagerClass = 'isotope-pager';
var itemSelector = ".single-product-wrapper";
var $buttonGroup = $('.button-group');
var sortByValue;
$('.item_sorting').on('click', '.product_sorting_btn', function (event) {
    sortByValue = $(event.currentTarget).attr('data-sort-by');
    sortAllWithPegination();
    goToPage(1);
});

var $container = $('#productList').isotope({
    itemSelector: itemSelector,
    getSortData: {
        Default: function (x) {
            var text = $(x).find('.product-item')
            var textOnly = text.text()
            return parseFloat(textOnly);
        },
        LowToHigh: function (x) {
            var text = $(x).find('.product-price')
            var textOnly = text.text().replace('$', '');
            return parseFloat(textOnly);
        },
        HighToLow: function (x) {
            var text = $(x).find('.product-price')
            var textOnly = text.text().replace('$', '');
            return parseFloat(textOnly);
        }
    },
    sortAscending: {
        Default: true,
        LowToHigh: true,
        HighToLow: false
    },
    sortBy: sortByValue

});


// update items based on current filters
function changeFilter(selector) { $container.isotope({ filter: selector }); }


function sortAllWithPegination() {
    $container.children(itemSelector).each(function () {
        var classes = $(this).attr('class').split(' ');
        var lastClass = classes[classes.length - 1];
        // last class shorter than 3 will be a page number, if so, grab and replace
        if (lastClass.length < 3) {
            classes.pop();
            classes = classes.join(' ');
            $(this).removeClass();
            $(this).addClass(classes);
        }
    });

    $container.isotope({
        filter: currentFilter,
        sortBy: sortByValue
    });

    var item = 0;
    var page = 1;
    var temp = 1;
    $container.children(itemSelector).each(function () {
        if (temp > itemsPerPage) {
            page++;
            temp = 1;
        }
        var wordPage = page.toString();
        var elem = $container.isotope('getFilteredItemElements');
        $(elem[item]).addClass(wordPage);
        //console.log(elem[item]);
        item++;
        temp++;
    });
}

$container.imagesLoaded(function () {
    // images have loaded
    setPagination();
    goToPage(1);
});

//event handlers

// store filter for each group
var filters = {};

$buttonGroup.on('click', '.button', function (event) {
    var $button = $(event.currentTarget);
    // get group key
    var $thisButtonGroup = $button.parents('.button-group');
    // 不同filter屬性
    var filterGroup = $thisButtonGroup.attr('data-filter-group');
    // set filter for group, 把屬性灌進去
    filters[filterGroup] = $button.attr('data-filter');
    // combine filters
    currentFilter = concatValues(filters);
    setPagination();
    goToPage(1);
});

$buttonGroup.each(function (i, buttonGroup) {
    var $thisButtonGroup = $(buttonGroup);
    $thisButtonGroup.on('click', '.button', function (event) {
        $thisButtonGroup.find('.is-checked').removeClass('is-checked');
        var $button = $(event.currentTarget);
        $button.addClass('is-checked');
    });
});

// remove checks from all boxes and refilter
$('#clear-filters').click(function () { clearAll() });

function clearAll() {
    $buttonGroup.each(function (i, buttonGroup) {
        var $thisButtonGroup = $(buttonGroup);
        $thisButtonGroup.find('.is-checked').removeClass('is-checked');
    });
    currentFilter = '*';
    filters = {};
    sortByValue = 'Default';
    sortAllWithPegination();
    setPagination();
    goToPage(1);
}

$(window).resize(function () {
    setPagination();
    goToPage(1);
});

function concatValues(obj) {
    var value = '';
    for (var prop in obj) {
        value += obj[prop];
    }
    return value;
}

var iso = $('#productList').data('isotope');
function changeCountValue() {
    if (currentFilter == '*') {
        var filterCount = $(itemSelector).length;
        $('#countOfIsotopeItem').text(filterCount.toString());
    }
    else if (iso.filteredItems.length < 12) {
        $('#countOfIsotopeItem').text(iso.filteredItems.length.toString());
    }
    else {
        var filterCount = $(currentFilter).length;
        $('#countOfIsotopeItem').text(filterCount.toString());
    }
}



