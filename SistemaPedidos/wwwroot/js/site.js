window.abrirComandaImpresion = function (pedidoId) {
    const win = window.open(`/pedidos/comanda/${pedidoId}`, "_blank", "width=400,height=800");

    if (!win) return;

    const timer = setInterval(() => {
        try {
            if (win.document.readyState === "complete") {
                clearInterval(timer);
                win.focus();
                win.print();
            }
        } catch { }
    }, 500);
};