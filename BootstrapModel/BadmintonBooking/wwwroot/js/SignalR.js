(function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/sessionHub")
        .build();

    connection.on("ReceiveLogoutNotification", () => {
        window.location.href = "/Account/Login";
    });

    connection.start().catch(err => console.error('Error while starting connection: ' + err));
})();