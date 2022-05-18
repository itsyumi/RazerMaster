'use strict';
const storeInfo = [{
        storeName: 'RAZERMASTER STORE LONDON',
        city: 'Charing Cross',
        position: 'Unit 7, 53 Charing Cross Road, London,WC2H 7PZ, United Kingdom',
        img: 'LondonStore.png'
    },
    {
        storeName: 'RAZERMASTER STORE LAS VEGAS',
        city: 'The Linq Promenade',
        position: '3545 S. Las Vegas Blvd. L-27, Las Vegas,NV 89109',
        img: 'Las_vegassStore.png'
    },
    {
        storeName: 'RAZERMASTER STORE HONG KONG',
        city: 'Causeway Bay',
        position: 'Shops A & D on G/F, No. 1 Cannon Street Causeway Bay, Hong Kong SAR',
        img: 'Hong_KongStore.png'
    }, {
        storeName: 'RAZERSTORE TAIPEI',
        city: 'Syntrend Creative Park',
        position: 'RazerStore Taipei, R207-1, 2F. No. 2,\
        Section 3, Civic Blvd Zhongshan District,\
        Taipei City 104',
        img: 'TaipeiStore.png'
    },
    {
        storeName: 'RAZERMASTER STORE SAN FRANCISCO',
        city: 'Westfield San Francisco Centre, Shop 136',
        position: '865 Market Street San Francisco,CA 94103',
        img: 'San_FranciscoStore.png'
    }
]
const storeDisplay = document.querySelector('.storeDisplay');
const store = document.querySelector('.storeInfo');


storeInfo.forEach(sI => {
    let clone = document.importNode(store.content, true);
    clone.querySelector('img').src = "../Assets/Img/about/store/"+sI.img;
    clone.querySelector('h3').textContent = sI.storeName;
    clone.querySelector('div>.text-white').textContent = sI.city;
    clone.querySelector('div>.text-grey').textContent = sI.position;
    storeDisplay.appendChild(clone);

});