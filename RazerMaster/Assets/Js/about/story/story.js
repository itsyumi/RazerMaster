const aboutData =
    [
        {
            title: "BEST OF CES",
            content: "Razer has won the Best of CES for an unprecedented seven years in a row, including People’s Choice Award, Best PC, Best Gaming Device and Best Concept, among others",
            img: "best-of-ces.jpg"
        },
        { title: "MICE", content: "Razer’s Mice have won numerous awards over the years from renowned publications including Best Wireless Gaming Mouse, Best Mouse for Gaming, Most Popular Mouse, Best Overall Mouse and numerous Editor’s Choice Awards.", img: "mice.jpg" },
        { title: "KEYBOARDS", content: "Razer’s range of gaming keyboards have been receiving recognition from publications including numerous Editor’s Choice awards, Best Gaming Keyboard, Best Mechanical Keyboard and Best Keyboard for Gaming.", img: "keyboard.jpg" },
        { title: "AUDIO", content: "Razer’s audio products have been winning awards across the board, such as Best Gaming Headset, Best Soundbar for Gaming, Platinum Award for Speakers, Speaker of the Year, Top Headset and numerous Editor’s Choice Awards.", img: "audio.jpg" },
        { title: "SYSTEMS", content: "The Razer Blade family of gaming laptops and ultrabooks have been winning awards for innovation and quality, including Best Laptop, Best Innovation, Best Ultraportable Laptop and numerous Editor’s Choice Awards.", img: "system.jpg" },
        { title: "GAMING LIFESTYLE", content: "Razer’s Gaming lifestyle products such as the Leviathan Mini and The Nabu Smartband have won awards from prestigious tech publications such as Stuff Magazine, Engadget and Tweaktown.", img: "gaming-lifestyle.jpg" }
    ]
const accolTemplateEl = document.querySelector(".accolades-item")
const accolEl = document.querySelector(".accolades-content")

aboutData.forEach(({ title, content, img }) => {
    let clone = accolTemplateEl.content.cloneNode(true)
    clone.querySelector("h4").textContent = title
    clone.querySelector("p").textContent = content
    clone.querySelector("img").src = "../Assets/Img/about/story/" + img
    accolEl.append(clone)
})
