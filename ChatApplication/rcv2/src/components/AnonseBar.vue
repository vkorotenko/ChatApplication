<template>
    <div class="anonse_bar">
        <div class="row">
            <div class="col-md-2 mp-0">
                <img src="/api/v1/article/53/img" class="w48r3">
            </div>
            <div class="col-md-10 mp-0">
                <div class="anonse_header">
                    Капот для ВАЗ <br>
                    Артикул: Капот<br>
                    Производитель: Стрелка 11 <br>
                    Цена: 42
                    <img src="/img/rc/rub13.png" style="height: 11px;">
                    <a href="/article/view?id=53">к обьявлению </a>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: 'AnonseBar',
        props: {
            //query: String
        }
    }
</script>
<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>