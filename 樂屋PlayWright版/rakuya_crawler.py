import json
import math
from playwright.sync_api import sync_playwright
from bs4 import BeautifulSoup

class RakuyaCrawler:
    def __init__(self, headless=False):
        self.playwright = sync_playwright().start()
        self.browser = self.playwright.chromium.launch(headless=headless)
        self.context = self.browser.new_context(
            locale="zh-TW",
            user_agent=(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) "
                "AppleWebKit/537.36 (KHTML, like Gecko) "
                "Chrome/126.0.0.0 Safari/537.36"
            )
        )
        self.page = self.context.new_page()

        self.page.route(
            "**/*",
            lambda route: route.abort() 
            if route.request.resource_type in ["image", "stylesheet", "font", "media"] 
            else route.continue_()
        )

        self.page.goto("https://www.rakuya.com.tw/search/sell_search")

    def fetch_page(self, zipcode, page_no):
        url = (
            "https://www.rakuya.com.tw/search/sell_search/get-result"
            f"?zipcode={zipcode}&sort=21&page={page_no}&upd=0"
        )

        response_text = self.page.evaluate(
            """(url) => {
                const controller = new AbortController();
                setTimeout(() => controller.abort(), 10000);

                return fetch(url, { signal: controller.signal })
                    .then(r => r.text())
                    .catch(err => "");
            }""",
            url
        )

        data = json.loads(response_text)
        html = data.get("list", "")
        
        soup = BeautifulSoup(html, "lxml")

        return soup.find_all("section", class_="grid-item search-obj")

    def close(self):
        self.browser.close()
        self.playwright.stop()
        
    def fetch_detail_soup(self, url):
        self.page.goto(url, wait_until="domcontentloaded")
        soup = BeautifulSoup(self.page.content(), "lxml")
        return soup
    
    def get_map(self, oid):
        url = (
            "https://www.rakuya.com.tw/"
            f"sell_item/api/item-environment/list?ehid={oid}"
        )

        text = self.page.evaluate(
            """(url) => fetch(url).then(r => r.text())""",
            url
        )

        return json.loads(text)
    
    def get_total(self, zipCode):
        url = (
            "https://www.rakuya.com.tw/"
            f"sell-search/api/search-item/count?zipcode={zipCode}"
        )
        response_text = self.page.evaluate(
            """(url) => {
                const controller = new AbortController();
                setTimeout(() => controller.abort(), 10000);

                return fetch(url, {
                    headers: {
                        "Accept": "application/json",
                        "X-Requested-With": "XMLHttpRequest"
                    },
                    signal: controller.signal
                })
                .then(r => r.text())
                .catch(() => "");
            }""",
            url
        )

        if not response_text:
            raise RuntimeError("get_total fetch failed")

        data = json.loads(response_text)
        total = data.get("data", {}).get("count", 0)

        # Rakuyo 一頁 19 筆
        return math.ceil(total / 19) if total else 0