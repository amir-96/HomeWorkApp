// Sidebar button

function toggleSidebar() {
  const sidebar = document.getElementById("sidebar");
  const backdrop = document.getElementById("backdrop");
  const body = document.body; // Get the body element

  if (sidebar.classList.contains("right-[-256px]")) {
    sidebar.classList.remove("right-[-256px]");
    sidebar.classList.add("right-0");
    backdrop.classList.remove("hidden");
    body.classList.add("overflow-hidden");
  } else {
    sidebar.classList.remove("right-0");
    sidebar.classList.add("right-[-256px]");
    backdrop.classList.add("hidden");
    body.classList.remove("overflow-hidden");
  }
}

// Sidebar dropdown

function toggleAdmin() {
  const dropdown = document.getElementById("adminNav");
  const dropdownIcon = document.getElementById("adminNavIcon");

  dropdown.classList.toggle("hidden");
  dropdownIcon.classList.toggle("rotate-[180]");
}

function toggleTeacher() {
  const dropdown = document.getElementById("teacherNav");
  const dropdownIcon = document.getElementById("teacherNavIcon");

  dropdown.classList.toggle("hidden");
  dropdownIcon.classList.toggle("rotate-[180]");
}

function toggleStudent() {
  const dropdown = document.getElementById("studentNav");
  const dropdownIcon = document.getElementById("studentNavIcon");

  dropdown.classList.toggle("hidden");
  dropdownIcon.classList.toggle("rotate-[180]");
}

setTimeout(function () {
  var successNotification = document.getElementById('successNotification');
  successNotification.style.opacity = '0';
  setTimeout(function () {
    successNotification.style.display = 'none';
  }, 500);
}, 4000);

setTimeout(function () {
  var failureNotification = document.getElementById('failureNotification');
  failureNotification.style.opacity = '0';
  setTimeout(function () {
    failureNotification.style.display = 'none';
  }, 500);
}, 4000);