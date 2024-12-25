//===================== begin::OverlayScrollbars Configure =====================
const SELECTOR_SIDEBAR_WRAPPER = ".sidebar-wrapper";
const Default = {
    scrollbarTheme: "os-theme-dark",
    scrollbarAutoHide: "leave",
    scrollbarClickScroll: true,
};
document.addEventListener("DOMContentLoaded", function () {
    const sidebarWrapper = document.querySelector(SELECTOR_SIDEBAR_WRAPPER);
    if (
        sidebarWrapper &&
        typeof OverlayScrollbarsGlobal?.OverlayScrollbars !== "undefined"
    ) {
        OverlayScrollbarsGlobal.OverlayScrollbars(sidebarWrapper, {
            scrollbars: {
                theme: Default.scrollbarTheme,
                autoHide: Default.scrollbarAutoHide,
                clickScroll: Default.scrollbarClickScroll,
            },
        });
    }
});
//===================== end::OverlayScrollbars Configure =====================
$(document).ready(function () {
    //================ begin::Sidebar Status =====================
    if (localStorage.getItem("SidbarStatus") == 'open') {
        $('body').removeClass('sidebar-collapse').addClass('sidebar-open');
    } else {
        $('body').removeClass('sidebar-open').addClass('sidebar-collapse');
    }

    $('a.nav-link[data-lte-toggle="sidebar"]').click(function () {
        setTimeout(function () {
            if ($('body').hasClass('sidebar-open')) {
                localStorage.setItem("SidbarStatus", 'open');
            } else {
                localStorage.setItem("SidbarStatus", 'close');
            }
        }, 500);
    });
    //================ end::Sidebar Status =====================
});
//================ DataTable =====================
$(document).ready(function () {
    var table = $("#tbl").DataTable({
        order: [],
        language: {
            url: "/admin/plugins/DataTables/vi.json",
        },
        columnDefs: [
            {
                targets: "noSort",
                orderable: false,
            },
        ],
    });
});

//================ Custom Toastr =====================
$(document).ready(function () {
    toastr.options = {
        "closeButton": true,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toast-top-center", // Vị trí hiển thị (top-right, top-left, bottom-right, bottom-left)
        "preventDuplicates": true,
        "timeOut": "5000", // Thời gian tồn tại (ms)
    };
});