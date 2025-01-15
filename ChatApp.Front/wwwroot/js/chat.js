// SignalR bağlantısını başlat
const connection = new signalR.HubConnectionBuilder().withUrl("http://localhost:5221/chathub").build();

// SignalR ile gelen mesajları dinle
connection.on("Messages", function (message) {
    const msgClass = message.userId === parseInt(currentUserId) ? 'sent' : 'received';
    $("#chatWindow").append(`<div class='message ${msgClass}'><p>${message.message}</p><span>${new Date(message.date).toLocaleTimeString()}</span></div>`);
    $("#chatWindow").scrollTop($("#chatWindow").prop("scrollHeight")); // Chat ekranını aşağı kaydır
});

connection.start()
        .then(() => console.log("SignalR connected"))
        .catch(err => console.error("SignalR connection error:", err));

const currentUserId = document.getElementById('userData').getAttribute('data-user-id');
console.log("Current User ID:", currentUserId);


// Kullanıcı listesini yükle
function loadUsers() {
    console.log("Loading users...");
    $.get(`http://localhost:5221/api/Chats/GetUsers?currentUserId=${currentUserId}`, function(users) {
        const userList = $("#userList");
        userList.empty();
        users.forEach(user => {
            userList.append(`<li class='user-item' data-id='${user.id}'>${user.name}</li>`);
        });
    }).fail(function() {
        console.error("Failed to load users.");
    });
}

loadUsers();

// Mesajları yükle
function loadMessages(userId, toUserId) {
    console.log("Loading messages...");
    $.get(`http://localhost:5221/api/Chats/GetChats?UserId=${userId}&ToUserId=${toUserId}`, function(messages) {
        const chatWindow = $("#chatWindow");
        chatWindow.empty();
        messages.forEach(msg => {
            const msgClass = msg.userId === parseInt(userId) ? 'sent' : 'received';
            chatWindow.append(`<div class='message ${msgClass}'><p>${msg.message}</p><span>${new Date(msg.date).toLocaleTimeString()}</span></div>`);
        });
        chatWindow.scrollTop(chatWindow.prop("scrollHeight"));
    }).fail(function() {
        console.error("Failed to load messages.");
    });
}

// Mesaj gönderme
$("#sendButton").on("click", function () {
    const message = $("#messageInput").val();
    const toUserId = $(".user-item.active").data("id");
    const userId = currentUserId;

    if (!toUserId || !message.trim()) {
        alert("Please select a user and write a message.");
    return;
}

$.ajax({
    url: "http://localhost:5221/api/Chats/SendMessage",
    type: "POST",
    contentType: "application/json",
    data: JSON.stringify({
        UserId: userId,
        ToUserId: toUserId,
        Message: message
    }),
    success: function () {
        $("#chatWindow").append(`<div class='message sent'><p>${message}</p><span>${new Date().toLocaleTimeString()}</span></div>`);
        $("#messageInput").val("");
        $("#chatWindow").scrollTop($("#chatWindow").prop("scrollHeight"));
    },
    error: function (err) {
        console.error("Failed to send message:", err);
        }
    });
});

// Kullanıcı tıklandığında mesajları yükle
$(document).on("click", ".user-item", function () {
    const toUserId = $(this).data("id");
    const userId = currentUserId;

    console.log(`Loading chat with User ID: ${toUserId}`);
    $(".user-item").removeClass("active");
    $(this).addClass("active");

    loadMessages(userId, toUserId);
});

// Profil butonu
$("#profileButton").on("click", function () {
    window.location.href = "@Url.Action("RedirectToProfile", "Home")";
});