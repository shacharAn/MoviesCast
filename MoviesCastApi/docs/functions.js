const apiBase   = "https://localhost:7153/api";
const apiMovies = `${apiBase}/movies`;
const apiWish   = `${apiBase}/wishlist`;
const apiCasts  = `${apiBase}/casts`;
const apiUsers  = `${apiBase}/users`;

let allMoviesCache = [];
let seeded = false;

function renderInto(containerId, movies, withAddButton = false) {
  const $c = $("#" + containerId).empty();

  if (!movies || movies.length === 0) {
    $c.html("<p>אין פריטים להצגה.</p>");
    return;
  }

  const seen = new Set();
  const uniqueMovies = [];
  movies.forEach(m => {
    const title = (m.title ?? m.Title ?? "").trim();
    const year  = m.releaseYear ?? m.ReleaseYear ?? 0;
    const key   = `${title}#${year}`;

    if (!seen.has(key)) {
      seen.add(key);
      uniqueMovies.push(m);
    }
  });

  uniqueMovies.forEach(m => {
    const id     = m.id ?? m.Id;
    const title  = m.title ?? m.Title;
    const photo  = m.photoUrl ?? m.PhotoUrl;
    const rating = m.rating ?? m.Rating;
    const genre  = m.genre ?? m.Genre;
    const lang   = m.language ?? m.Language;

    const card = $(`
      <div class="movie-card">
        <img src="${photo}" alt="${title}">
        <div class="info">
          <h3>${title}</h3>
          <p> ${rating} | ${genre} | ${lang}</p>
          ${
            withAddButton
              ? `<button class="btn-add-wish" data-id="${id}"> הוסף ל-Wish List</button>`
              : ``
          }
        </div>
      </div>
    `);
    $c.append(card);
  });
}


async function loadMoviesFromFile() {
  try {
    const res = await fetch(apiMovies);
    if (!res.ok) throw new Error("movies api error");

    const moviesFromDb = await res.json();

    allMoviesCache = moviesFromDb;

    renderInto("all-movies", moviesFromDb, /*withAddButton*/ true);
  } catch (err) {
    console.error("שגיאה בטעינת הסרטים", err);
    $("#all-movies").html("<p> שגיאה בטעינת הסרטים</p>");
  }
}


async function seedMoviesOnServer(movies) {
  for (const m of movies) {
    try {
      await $.ajax({
        type: "POST",
        url: apiMovies,
        contentType: "application/json",
        data: JSON.stringify({
          id: m.id, title: m.title, rating: m.rating, income: m.income,
          releaseYear: m.releaseYear, duration: m.duration, language: m.language,
          description: m.description, genre: m.genre, photoUrl: m.photoUrl
        })
      });
    } catch (xhr) {
      if (xhr.status !== 409) console.warn("seed error:", xhr);
    }
  }
}

function addToWishListById(movieId) {
  const user = getLoggedUser();
  if (!user) {
    alert("רק משתמש מחובר יכול להוסיף ל-Wish List");
    return;
    }

  let title = "הסרט";
  const m = allMoviesCache.find(x => (x.id ?? x.Id) === movieId);
  if (m) {
    title = m.title ?? m.Title ?? title;
  }
  $.ajax({
    type: "POST",
    url: apiWish,
    contentType: "application/json",
    data: JSON.stringify({ id: movieId }),
    success: () => alert(` "${title}" נוסף ל-Wish List`),
    error: (xhr) => {
      if (xhr.status === 409) {
        alert("כבר קיים ב-Wish List או לא נמצא.");
      } else {
        alert("שגיאה בהוספה ל-Wish List.");
      }
    }
  });
}

function loadWishList() {
  $.get(apiWish, (data) => renderInto("wish-movies", data, /*withAddButton*/ false))
  .fail(() => $("#wish-movies").html("<p> שגיאה בטעינת ה-Wish List</p>"));
}

function filterByRating() {
  const val = document.getElementById('ratingInput').value;
  const rating = Number(val);

  if (Number.isNaN(rating) || rating < 0 || rating > 10) {
    alert('אנא הזיני ערך דירוג בין 0 ל-10');
    return;
  }

  fetch(`${apiMovies}/rating/${encodeURIComponent(rating)}`)
    .then(r => r.ok ? r.json() : Promise.reject(r))
    .then(data => renderInto('all-movies', data, /*withAddButton*/ true))
    .catch(err => {
      console.error('rating filter error', err);
      alert('שגיאה בסינון לפי דירוג');
    });
}

function filterByDuration() {
  const val = document.getElementById('durationInput').value;
  const duration = parseInt(val, 10);

  if (Number.isNaN(duration) || duration < 1) {
    alert('אנא הזיני משך תקין (בדקות), לפחות 1');
    return;
  }

  fetch(`${apiMovies}/duration?maxDuration=${encodeURIComponent(duration)}`)
    .then(r => r.ok ? r.json() : Promise.reject(r))
    .then(data => renderInto('all-movies', data, /*withAddButton*/ true))
    .catch(err => {
      console.error('duration filter error', err);
      alert('שגיאה בסינון לפי משך');
    });
}


function renderCastList(casts) {
  const $c = $("#cast-list").empty();
  if (!casts || casts.length === 0) {
    $c.html("<p>אין שחקנים להצגה.</p>");
    return;
  }
  casts.forEach(c => {
    const name  = c.name ?? c.Name;
    const role  = c.role ?? c.Role;
    const age   = c.age ?? c.Age;
    const photo = c.photoUrl ?? c.PhotoUrl;
    const card = $(`
      <div class="movie-card">
        <img src="${photo || 'https://via.placeholder.com/400x300?text=Cast'}" alt="${name}">
        <div class="info">
          <h3>${name}</h3>
          <p>${role || ''}${age ? ' | גיל ' + age : ''}</p>
        </div>
      </div>
    `);
    $c.append(card);
  });
}

function loadCasts() {
  $.get(apiCasts, (data) => renderCastList(data))
  .fail(() => $("#cast-list").html("<p> שגיאה בטעינת השחקנים</p>"));
}

function validateCastForm() {
  let ok = true;

  $("#errId, #errName, #errRole, #errAge, #errMovieId, #errPhoto").text("");

  const id       = Number($("#castId").val());
  const name     = ($("#castName").val() || "").trim();
  const role     = ($("#castRole").val() || "").trim();
  const ageStr   = $("#castAge").val();
  const movieStr = $("#castMovieId").val();
  const photo    = ($("#castPhoto").val() || "").trim();

  if (!id || id < 1) { $("#errId").text("חובה להזין מזהה חוקי (מעל 0)."); ok = false; }
  if (name.length < 2) { $("#errName").text("שם חייב להכיל לפחות 2 תווים."); ok = false; }
  if (role.length < 2) { $("#errRole").text("תפקיד חייב להכיל לפחות 2 תווים."); ok = false; }

  if (ageStr) {
    const age = Number(ageStr);
    if (Number.isNaN(age) || age < 0 || age > 120) { $("#errAge").text("גיל בין 0 ל-120."); ok = false; }
  }
  if (movieStr) {
    const mv = Number(movieStr);
    if (Number.isNaN(mv) || mv < 1) { $("#errMovieId").text("মזהה סרט חייב להיות מספר > 0."); ok = false; }
  }
  if (photo && !/^https?:\/\//i.test(photo)) {
    $("#errPhoto").text("יש להזין כתובת URL תקינה (http/https)."); ok = false;
  }

  return ok;
}
function validateMovieForm() {
  let ok = true;

  // איפוס הודעות שגיאה
  $("#errMovieTitle, #errMovieRating, #errMovieIncome, #errMovieYear, #errMovieDuration, #errMovieLanguage, #errMovieGenre, #errMoviePhoto, #errMovieDescription").text("");

  const title       = ($("#movieTitle").val() || "").trim();
  const ratingStr   = $("#movieRating").val();
  const incomeStr   = $("#movieIncome").val();
  const yearStr     = $("#movieYear").val();
  const durationStr = $("#movieDuration").val();
  const lang        = ($("#movieLanguage").val() || "").trim();
  const genre       = ($("#movieGenre").val() || "").trim();
  const photo       = ($("#moviePhoto").val() || "").trim();
  const desc        = ($("#movieDescription").val() || "").trim();

  if (title.length < 2) {
    $("#errMovieTitle").text("שם הסרט חייב להכיל לפחות 2 תווים.");
    ok = false;
  }

  if (ratingStr === "") {
    $("#errMovieRating").text("חובה להזין דירוג.");
    ok = false;
  } else {
    const rating = Number(ratingStr);
    if (Number.isNaN(rating) || rating < 0 || rating > 10) {
      $("#errMovieRating").text("דירוג חייב להיות בין 0 ל-10.");
      ok = false;
    }
  }

  if (incomeStr === "") {
    $("#errMovieIncome").text("חובה להזין הכנסות.");
    ok = false;
  } else {
    const income = Number(incomeStr);
    if (Number.isNaN(income) || income < 0) {
      $("#errMovieIncome").text("הכנסות חייבות להיות מספר חיובי.");
      ok = false;
    }
  }

  if (yearStr === "") {
    $("#errMovieYear").text("חובה להזין שנת יציאה.");
    ok = false;
  } else {
    const year = Number(yearStr);
    if (Number.isNaN(year) || year < 1900 || year > 2100) {
      $("#errMovieYear").text("שנת יציאה בין 1900 ל-2100.");
      ok = false;
    }
  }

  if (durationStr === "") {
    $("#errMovieDuration").text("חובה להזין משך.");
    ok = false;
  } else {
    const dur = Number(durationStr);
    if (Number.isNaN(dur) || dur < 1) {
      $("#errMovieDuration").text("המשך חייב להיות לפחות דקה אחת.");
      ok = false;
    }
  }

  if (lang.length < 2) {
    $("#errMovieLanguage").text("שפה חייבת להכיל לפחות 2 תווים.");
    ok = false;
  }

  if (genre.length < 2) {
    $("#errMovieGenre").text("ז'אנר חייב להכיל לפחות 2 תווים.");
    ok = false;
  }

  if (photo && !/^https?:\/\//i.test(photo)) {
    $("#errMoviePhoto").text("יש להזין כתובת URL תקינה (http/https).");
    ok = false;
  }

  if (desc.length < 5) {
    $("#errMovieDescription").text("התיאור חייב להכיל לפחות 5 תווים.");
    ok = false;
  }

  return ok;
}
function submitMovieForm() {
  const user = getLoggedUser();
  if (!user) {
    alert("רק משתמש מחובר יכול להוסיף סרט חדש.");
    return;
  }

  if (!validateMovieForm()) return;

  const payload = {
    title:       ($("#movieTitle").val() || "").trim(),
    rating:      Number($("#movieRating").val()),
    income:      Number($("#movieIncome").val()),
    releaseYear: Number($("#movieYear").val()),
    duration:    Number($("#movieDuration").val()),
    language:    ($("#movieLanguage").val() || "").trim(),
    description: ($("#movieDescription").val() || "").trim(),
    genre:       ($("#movieGenre").val() || "").trim(),
    photoUrl:    ($("#moviePhoto").val() || "").trim()
  };

  $.ajax({
    type: "POST",
    url: apiMovies,
    contentType: "application/json",
    data: JSON.stringify(payload),
    success: () => {
      alert(" הסרט נוסף בהצלחה!");
      $("#movieForm")[0].reset();
      loadMoviesFromFile(); 
    },
    error: (xhr) => {
      if (xhr.status === 409) {
        alert("סרט כזה כבר קיים במערכת.");
      } else {
        alert("שגיאה בהוספת סרט.");
      }
    }
  });
}

function submitCastForm() {
  if (!validateCastForm()) return;

  const payload = {
    id:       Number($("#castId").val()),
    name:     ($("#castName").val() || "").trim(),
    role:     ($("#castRole").val() || "").trim(),
    age:      $("#castAge").val() ? Number($("#castAge").val()) : 0,
    movieId:  $("#castMovieId").val() ? Number($("#castMovieId").val()) : 0,
    photoUrl: ($("#castPhoto").val() || "").trim()
  };

  $.ajax({
    type: "POST",
    url: apiCasts,
    contentType: "application/json",
    data: JSON.stringify(payload),
    success: () => {
      alert(" השחקן נוסף בהצלחה!");
      $("#castForm")[0].reset();
      loadCasts();
    },
    error: (xhr) => {
      if (xhr.status === 409) alert("שחקן עם אותו Id כבר קיים.");
      else alert("שגיאה בהוספת שחקן.");
    }
  });
}

function saveLoggedUser(user) {
  localStorage.setItem("loggedUser", JSON.stringify(user));
  updateUserStatus();
}

function getLoggedUser() {
  let u = localStorage.getItem("loggedUser");
  return u ? JSON.parse(u) : null;
}

function logoutUser() {
  localStorage.removeItem("loggedUser");
  updateUserStatus();
  alert("התנתקת בהצלחה");
}

function updateUserStatus() {
  const user = getLoggedUser();

  if (!user) {
    $("#userStatus").text("אורח");
    $("#btnShowLogin").show();
    $("#btnShowRegister").show();
    $("#btnLogout").hide();
  } else {
    $("#userStatus").text(`מחובר: ${user.userName}`);
    $("#btnShowLogin").hide();
    $("#btnShowRegister").hide();
    $("#btnLogout").show();
  }
}

function showLogin() { $("#loginModal").show(); }
function hideLogin() { $("#loginModal").hide(); }
function showRegister() { $("#registerModal").show(); }
function hideRegister() { $("#registerModal").hide(); }

function doLogin() {
  const email = $("#loginEmail").val().trim();
  const pass  = $("#loginPassword").val().trim();
  $("#loginError").text("");

  $.ajax({
    type: "POST",
    url: `${apiUsers}/login`,
    contentType: "application/json",
    data: JSON.stringify({ email, password: pass }),
    success: (u) => {
      saveLoggedUser(u);
      hideLogin();
      alert(" התחברת בהצלחה!");
    },
    error: () => {
      $("#loginError").text("פרטי התחברות שגויים.");
    }
  });
}

function doRegister() {
  const userName = $("#regUserName").val().trim();
  const email    = $("#regEmail").val().trim();
  const pass     = $("#regPassword").val().trim();
  $("#registerError").text("");

  $.ajax({
    type: "POST",
    url: `${apiUsers}/register`,
    contentType: "application/json",
    data: JSON.stringify({ userName, email, password: pass }),
    success: (u) => {
      alert(" נרשמת בהצלחה!");
      hideRegister();
    },
    error: (xhr) => {
      $("#registerError").text(xhr.responseText || "שגיאה בהרשמה");
    }
  });
}
