async function createBook() {
    const content = document.getElementById('content').value;
    const titles = document.getElementById('titles').value;
    const data = {
        content: content,
        titles: titles
    };

    const response = await fetch('/Books', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    });

    if (response.ok) {
        alert('Book created successfully!');
    } else {
        alert('Error creating book');
    }
}

async function getTitles() {
    const response = await fetch('/Books/GetTitles');
    const titles = await response.json();

    const titlesList = document.getElementById('titlesList');
    titlesList.innerHTML = titles.map(title => `<li>${title}</li>`).join('');
}

async function getTopTenWords() {
    const bookId = document.getElementById('bookId').value;
    const response = await fetch(`/Books/GetTopTenWords/${bookId}`);
    const topWords = await response.json();

    const topWordsList = document.getElementById('topWordsList');
    topWordsList.innerHTML = topWords.map(word => `<li>${word}</li>`).join('');
}

async function checkMatchingWord() {
    const bookId = document.getElementById('checkWordId').value;
    const wordToCheck = document.getElementById('wordToCheck').value;

    const response = await fetch(`/Books/IsMatchingWord/${bookId}/${wordToCheck}`);
    const result = await response.json();

    const matchingResult = document.getElementById('matchingResult');
    matchingResult.innerHTML = result
        ? 'The word matches the top words.'
        : 'The word does not match the top words.';
}

async function findSubstring() {
    const bookId = document.getElementById('substringId').value;
    const substring = document.getElementById('substring').value;

    const response = await fetch(`/Books/FindSubstring/${bookId}/${substring}`);
    const words = await response.json();

    const substringList = document.getElementById('substringList');
    substringList.innerHTML = words.map(word => `<li>${word}</li>`).join('');
}
