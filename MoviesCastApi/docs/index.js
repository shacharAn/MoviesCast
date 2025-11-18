function showTab(name) {
  document.querySelectorAll('.page').forEach(p => p.classList.remove('active'));
  if (name === 'all') {
    document.getElementById('page-all').classList.add('active');
    loadMoviesFromFile(); // רענון לפי הצורך
  } else if (name === 'wish') {
    document.getElementById('page-wish').classList.add('active');
    loadWishList();
  } else {
    document.getElementById('page-cast').classList.add('active');
    loadCasts();
  }
}

document.addEventListener('DOMContentLoaded', () => {
  document.getElementById('btnTabAll') .addEventListener('click', () => showTab('all'));
  document.getElementById('btnTabWish').addEventListener('click', () => showTab('wish'));
  document.getElementById('btnTabCast').addEventListener('click', () => showTab('cast'));

  document.getElementById('btnFilterRating').addEventListener('click', filterByRating);
  document.getElementById('btnFilterDuration').addEventListener('click', filterByDuration);

  document.getElementById('btnWishRefresh').addEventListener('click', loadWishList);

  document.getElementById('all-movies').addEventListener('click', (e) => {
    const btn = e.target.closest('.btn-add-wish');
    if (!btn) return;
    const id = Number(btn.dataset.id);
    if (!Number.isNaN(id)) addToWishListById(id);
  });

  document.getElementById('castForm').addEventListener('submit', (e) => {
    e.preventDefault();
    submitCastForm();
  });

  document.addEventListener('keydown', (e) => {
    if (e.key === 'Enter') {
      if (document.activeElement.id === 'ratingInput')   { filterByRating();   e.preventDefault(); }
      if (document.activeElement.id === 'durationInput') { filterByDuration(); e.preventDefault(); }
    }
  });

  loadMoviesFromFile();
});
