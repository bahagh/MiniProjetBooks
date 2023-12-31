// Function to create a book
async function createBook() {
    const content = document.getElementById('content').value;
    const titles = document.getElementById('titles').value;
    const data = {
        content: content,
        titles: titles
    };

    const response = await fetch('/api/Books', { 
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

// Function to get titles
document.getElementById('getTitlesBtn').addEventListener('click', function () {
    fetch('/api/Books')
        .then(response => response.json())
        .then(data => {
            var titlesContainer = document.getElementById('titlesContainer');
            titlesContainer.innerHTML = '';

            data.forEach(function (title) {
                var titleButton = document.createElement('button');
                titleButton.textContent = title;
                titleButton.addEventListener('click', function () {
                    // When a title button is clicked, trigger the getTopTenWords function
                    getTopTenWords(title);
                });
                titlesContainer.appendChild(titleButton);
            });
        });
});

// Function to trigger the getTopTenWords function
function getTopTenWords(title) {
    const id = parseInt(title.split('-')[0].trim());

    fetch(`/api/Books/${id}`)
        .then(response => response.json())
        .then(data => {
            // Display the list of words and their occurrences in an alert
            alert('Top Ten Words for "' + title + ':"\n' +
                data.map(wordInfo => `${wordInfo.key} - ${wordInfo.value}`).join('\n'));
        });
}

// Function to check if a word matches the top words
async function checkMatchingWord() {
    const bookId = document.getElementById('checkWordId').value;
    const wordToCheck = document.getElementById('wordToCheck').value;

    const response = await fetch(`/api/Books/${bookId}/${wordToCheck}`);
    const result = await response.json();

    const matchingResult = document.getElementById('matchingResult');
    matchingResult.innerHTML = result
        ? 'The word matches the top words.'
        : 'The word does not match the top words.';
}

// Function to find a substring in words
async function findSubstring() {
    const bookId = document.getElementById('substringId').value;
    const substringInput = document.getElementById('substring');
    const substring = substringInput.value;

    if (substring.length < 3) {
        alert('Please enter a substring with at least 3 characters.');
        return;
    }

    const response = await fetch(`/api/Books/${bookId}/search/${substring}`);
    const wordOccurrences = await response.json();

    const substringList = document.getElementById('substringList');
    substringList.innerHTML = wordOccurrences.map(wordOccurrence => {
        const { key, value } = wordOccurrence;
        return `<li>${key} - Occurrences: ${value}</li>`;
    }).join('');
}
