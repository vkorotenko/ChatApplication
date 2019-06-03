<?php

use yii\bootstrap\Nav;
use yii\bootstrap\NavBar;
use yii\helpers\Html;
use yii\widgets\ActiveForm;
use yii\httpclient\Client;
use yii\widgets\Pjax;
/* @var $this \yii\web\View */
/* @var $content string */

$bundle = \frontend\assets\FrontendAsset::register($this);
//Yii::$app->params['sms'] = '0';

$this->beginContent('@frontend/views/layouts/_clear.php')
//$header = new CHeader();
?>


<header class="header">
	<div class="panel"> 
		<div class="case">
			<div class="panel__wrap row i-mid">
				<div class="panel__logo"><a href="/"><img src="/upload/logo.png" alt=""></a></div>
				<div class="panel__nav hidden-md hidden-sm hidden-xs">
				
						<?	if(Yii::$app->user->isGuest){?>		
					<ul class="scroll-block">
						<li><a href="#about">О компании</a></li>
						<li><a href="#steps">Схема работы</a></li>
						<li><a href="#advantages">Наши преимущества</a></li>
						<li><a href="#services">Услуги</a></li>
						<li><a href="#reviews">Отзывы</a></li>
						<li><a href="#contacts">Контакты</a></li>
					</ul>
									
	<?}else{?>			
					<ul>
						<li><a href="/article/">Объявления</a></li>
						<li><a href="">Комиссионный склад</a></li>
						<li><a href="/user/article">Виртуальный склад</a></li>
						<li><a href="/account/orders">Заказы</a></li>
						<li><a href="">Избранное</a></li>
					</ul>
		<?}?>
				</div>
				<?	if(Yii::$app->user->isGuest){}else{	?>
				<div class="panel__lk row i-mid">
					<div class="panel__cart lk-link hidden-xs">
						<div class="lk-link__icon"><a href=""><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24" height="22" viewBox="0 0 24 22"><defs><path id="f7q1a" d="M1028 17l1 2h17c.69 0 1.47.28 1.47.97 0 .23 0 .34-.22.57L1043.1 29c-.34.69-1.2 1-2.11 1h-8l-1.36 2.1v.11c0 .12.25-.21.36-.21h13v2h-14c-1.26 0-1.88-.3-1.88-1.56 0-.34.11-.8.22-1.15l1.61-2.75L1027 19h-3v-2zm4 18a2 2 0 1 1 0 4 2 2 0 0 1 0-4zm10 0a2 2 0 1 1 0 4 2 2 0 0 1 0-4z"/><clipPath id="f7q1b"><use fill="#fff" xlink:href="#f7q1a"/></clipPath></defs><g><g transform="translate(-1024 -17)"><use fill="#fff" fill-opacity="0" stroke="#000" stroke-miterlimit="50" stroke-width="4" clip-path="url(&quot;#f7q1b&quot;)" xlink:href="#f7q1a"/></g></g></svg><span>4</span></a></div>
					</div>
					<div class="panel__alerts lk-link lk-link_has-hidden hidden-xs">
						<div class="lk-link__icon"><a href="buyer-alerts.html"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="22" height="23" viewBox="0 0 22 23"><defs><path id="z2yda" d="M1079 37h4c0 1.37-.63 2-2 2s-2-.63-2-2zm13-2v1h-22v-1.3l3-1.7v-8c0-3.86 3.38-6.13 7-7v-1c0-1 0-1 1-1s1 0 1 1v1c3.62.87 7 3.14 7 7v8zm-5-10c0-3.12-2.88-5-6-5s-6 1.88-6 5v9h12z"/></defs><g><g transform="translate(-1070 -16)"><use xlink:href="#z2yda"/></g></g></svg><span>12</span></a></div>
						<div class="lk-link__wrap">
							<div class="lk-link__wrap-title">уведомления</div>
							<div class="lk-link__wrap-load"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="14" height="14" viewBox="0 0 14 14"><g><g transform="translate(-1123 -110)"><image width="14" height="14" transform="translate(1123 110)" xlink:href="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAA4AAAAOCAYAAAAfSC3RAAABe0lEQVQ4T5WSvUvDUBTFz0vSJrHEpEhtanFzER2cHATt4NbFSXH03/A/EVzERQQHBymIDm4ibiLoIE5CjCUxSdPafJAnL9XaSuPH3d5793fe5dxDKKUUA+U4DoIgQBiG6W0+n4coitA0bbANhIF+TOEHMQLbRFEdhyAIKcCKCURRBN/3US6X0zdWJEoo3X0MsPPUxtGCghk1BzKk3TswAcuyUCqVUpjcugmdP/OALo/9moz1Cg+JH0F+wM1mE9VqFaR+6dLGAwdMC3hdEqHlRv33JeR5HpIkAcGJQ2HxuKiPYXmCA/czh06ng1arBYIDlyLgYG4WMCn+QgGI4xiGYYDg0KXocjA2CtCl/4CN3qjHqzLWKhmuDHjVH3Xr2qV79xwwxeOtJmc6+sm6rguWGXLnJXT23AO4HK5WZCwWs8dlu+yvox1Tevoc4ubFw/bceD8x3zfJINu20wDwPN+LHGtibpmmCUVRMiOn63oKpZH7S8glSYKqqkNDvAONtMA/jJk1eQAAAABJRU5ErkJggg=="/></g></g></svg></div>
							<div class="lk-link__wrap-list">
								<div class="lk-link__wrap-item">
									<div class="lk-link__wrap-text">Клиент заказал вашу деталь №3425483 и хочет заказать доставку на дом и оплатить наличными</div>
								</div>
								<div class="lk-link__wrap-item">
									<div class="lk-link__wrap-text">Клиент заказал вашу деталь №3425483 и хочет заказать доставку на дом и оплатить наличными</div>
								</div>
								<div class="lk-link__wrap-item">
									<div class="lk-link__wrap-text">Клиент заказал вашу деталь №3425483 и хочет заказать доставку на дом и оплатить наличными</div>
								</div>
								<div class="lk-link__wrap-item">
									<div class="lk-link__wrap-text">Клиент заказал вашу деталь №3425483 и хочет заказать доставку на дом и оплатить наличными</div>
								</div>
							</div>
							<a href="" class="lk-link__wrap-all">смотреть все уведомления</a>
						</div>
					</div>
					<div class="panel__messages lk-link lk-link_has-hidden hidden-xs" id="new-messages-informer">
                            <div class="lk-link__icon" style="display: none"><a href="buyer-messages.html"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="22" height="17" viewBox="0 0 22 17"><defs><path id="oh16a" d="M1142 19v17h-22V19zm-3 2h-16l8 7.06zm-17 2v10l6-5zm2 11h14l-5-5-2 1-2-1zm16-1V23l-6 5z" /></defs><g><g transform="translate(-1120 -19)"><use xlink:href="#oh16a" /></g></g></svg><span style="display: none" id="totalUnreadMessages"></span> </a></div>
                            <div class="lk-link__wrap">
                                <div class="lk-link__wrap-title">сообщения</div>                                
                                <div class="lk-link__wrap-list">
                                    <div class="lk-link__wrap-item" v-for="item in items">
	                                    <a :href="item.url">
	                                        <div class="lk-link__wrap-name">{{ item.fullName}}</div>
	                                        <div class="lk-link__wrap-text lk-link__wrap-text_small">{{ item.title}}</div>
	                                    </a>
                                    </div>                                    
                                </div>
                                <a href="/user/chat/" class="lk-link__wrap-all">смотреть все сообщения</a>
                            </div>
                        </div>
					
					<div class="panel__persona persona hidden-md hidden-sm hidden-xs">
<div class="persona__toggle"><img src="<?php 
echo Yii::$app->user->identity->userProfile->getAvatar($this->assetManager->getAssetUrl($bundle, 'img/anonymous.jpg')) ?>" alt=""><span><?echo  Yii::$app->user->isGuest ? '' : Yii::$app->user->identity->getPublicIdentity();
?></span></div>
						<div class="persona__drop">
							<div class="persona__drop-head row">
								<div class="persona__img">
<img src="<?php echo Yii::$app->user->identity->userProfile->getAvatar($this->assetManager->getAssetUrl($bundle, 'img/anonymous.jpg'));?>" alt=""></div>
								<div class="persona__text">
									<div class="persona__title"><?
					
								echo  Yii::$app->user->isGuest ? '' : Yii::$app->user->identity->getPublicIdentity();
								?></div>
<a href="/user/default/profile" class="persona__btn btn btn_shadow btn_bg-yellow btn_color-black btn_extra-small"><span>Смотреть профиль</span></a>
								</div>
							</div>
							<div class="persona__drop-body">
							<br>
<a href="/user/default/profile" class="persona__btn btn btn_shadow btn_bg-yellow btn_color-black btn_extra-small"><span>Покупатель</span></a>
<a href="/user/default/profile" class="persona__btn btn btn_shadow  btn_color-black btn_extra-small"><span>Продавец</span></a>							
								<div class="persona__list">
									<ul>
										<li><a href="/user/"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="17" height="16" viewBox="0 0 17 16"><defs><path id="75lva" d="M1077.21 171.75a.43.43 0 0 1-.45.38h-.1c-.68 0-1.32.42-1.59 1.05-.28.67-.1 1.45.44 1.94.17.15.19.41.05.59-.37.46-.8.88-1.26 1.24a.43.43 0 0 1-.59-.05c-.46-.5-1.28-.7-1.92-.43-.66.27-1.09.93-1.06 1.65 0 .23-.16.42-.38.45a7.7 7.7 0 0 1-1.75 0 .43.43 0 0 1-.38-.46c.04-.73-.38-1.4-1.05-1.69a1.78 1.78 0 0 0-1.94.44.43.43 0 0 1-.59.05 7.72 7.72 0 0 1-1.23-1.25.43.43 0 0 1 .05-.58c.53-.5.7-1.26.44-1.93a1.7 1.7 0 0 0-1.6-1.06c-.22 0-.49-.16-.51-.38a7.79 7.79 0 0 1 0-1.77c.03-.22.2-.39.45-.38a1.8 1.8 0 0 0 1.69-1.05c.28-.67.1-1.45-.43-1.94a.43.43 0 0 1-.05-.6c.37-.45.79-.87 1.26-1.23a.43.43 0 0 1 .58.05c.46.5 1.29.7 1.92.43a1.73 1.73 0 0 0 1.07-1.65c-.01-.23.15-.42.38-.45a7.74 7.74 0 0 1 1.75 0c.22.03.39.23.38.46-.04.73.38 1.4 1.05 1.69.64.27 1.47.07 1.93-.44a.43.43 0 0 1 .6-.05c.44.37.86.78 1.22 1.25.14.17.12.43-.05.58-.53.49-.7 1.26-.43 1.92.26.65.9 1.07 1.6 1.07.23 0 .48.16.5.38.07.59.07 1.18 0 1.77zm-5.1-.89a2.6 2.6 0 1 0-5.2 0 2.6 2.6 0 0 0 5.2 0z"/></defs><g><g transform="translate(-1061 -163)"><use fill="#afb7bf" xlink:href="#75lva"/></g></g></svg><span>Настройки</span></a></li>
										<li><a href="/user/sign-in/logout" data-method="post" tabindex="-1"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="14" height="14" viewBox="0 0 14 14"><defs><path id="dqcpa" d="M1063 205h7v2h-5v10h5v2h-7zm3 8v-2h7l-1.78-2.38 1.56-1.24 2.93 3.9 1.29.92-4.32 4.8-1.68-1.2 2.52-2.8z"/></defs><g><g transform="translate(-1063 -205)"><use fill="#afb7bf" xlink:href="#dqcpa"/></g></g></svg><span>Выйти</span></a></li>
									</ul>
								</div>
							</div>
						</div>
					</div>
				
				</div>
				<?}?>
					<?	if(Yii::$app->user->isGuest){?>
			<div class="panel__phone hidden-xs"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="12" height="20" viewBox="0 0 12 20" class="icon"><defs><path id="j8v1a" d="M1046 22v16c0 .88-1 1.98-1.88 1.98L1036 40c-.88 0-2-1.12-2-2V22c0-.88 1-1.98 1.88-1.98h8.24c.88 0 1.88 1.1 1.88 1.98zm-8-1v1h4l-.01-.62V21h-3.77zm4 0zm-1 17v-2h-2v2zm3-15h-8v12h8z"/></defs><g><g transform="translate(-1034 -20)"><use fill="#b1b1b1" xlink:href="#j8v1a"/></g></g></svg><a href="tel:+74956408854">8 495 640 88 54</a></div>
						
				<div class="panel__buy hidden-xs"></div>
				
				<?}?>
				<div class="panel__mobile-btn hidden-lg">
					<span></span>
					<span></span>
					<span></span>
				</div>
				<div class="panel__mobile-nav hidden-lg">
					<ul>
				
						<?	if(Yii::$app->user->isGuest){?>		
					<ul class="scroll-block">
						<li><a href="#about">О компании</a></li>
						<li><a href="#steps">Схема работы</a></li>
						<li><a href="#advantages">Наши преимущества</a></li>
						<li><a href="#services">Услуги</a></li>
						<li><a href="#reviews">Отзывы</a></li>
						<li><a href="#contacts">Контакты</a></li>
					</ul>
									
	<?}else{?>			
					<ul>
						<li><a href="/article/">Объявления</a></li>
						<li><a href="">Комиссионный склад</a></li>
						<li><a href="/user/article">Виртуальный склад</a></li>
						<li><a href="/account/orders">Заказы</a></li>
						<li><a href="">Избранное</a></li>
					</ul>
		<?}?>
						<?	if(Yii::$app->user->isGuest){?>	
						<li><a data-fancybox="" data-src="#popup-auth" href="#" >Войти в личный кабинет</a></li>	
						<?}else{?>
						<li class="mobile-profile">
							<a href="#">
<span><? echo  Yii::$app->user->isGuest ? '' : Yii::$app->user->identity->getPublicIdentity();
								?></span></a>
							<ul>
								<li><a href="/user/"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="17" height="16" viewBox="0 0 17 16"><defs><path id="75lva" d="M1077.21 171.75a.43.43 0 0 1-.45.38h-.1c-.68 0-1.32.42-1.59 1.05-.28.67-.1 1.45.44 1.94.17.15.19.41.05.59-.37.46-.8.88-1.26 1.24a.43.43 0 0 1-.59-.05c-.46-.5-1.28-.7-1.92-.43-.66.27-1.09.93-1.06 1.65 0 .23-.16.42-.38.45a7.7 7.7 0 0 1-1.75 0 .43.43 0 0 1-.38-.46c.04-.73-.38-1.4-1.05-1.69a1.78 1.78 0 0 0-1.94.44.43.43 0 0 1-.59.05 7.72 7.72 0 0 1-1.23-1.25.43.43 0 0 1 .05-.58c.53-.5.7-1.26.44-1.93a1.7 1.7 0 0 0-1.6-1.06c-.22 0-.49-.16-.51-.38a7.79 7.79 0 0 1 0-1.77c.03-.22.2-.39.45-.38a1.8 1.8 0 0 0 1.69-1.05c.28-.67.1-1.45-.43-1.94a.43.43 0 0 1-.05-.6c.37-.45.79-.87 1.26-1.23a.43.43 0 0 1 .58.05c.46.5 1.29.7 1.92.43a1.73 1.73 0 0 0 1.07-1.65c-.01-.23.15-.42.38-.45a7.74 7.74 0 0 1 1.75 0c.22.03.39.23.38.46-.04.73.38 1.4 1.05 1.69.64.27 1.47.07 1.93-.44a.43.43 0 0 1 .6-.05c.44.37.86.78 1.22 1.25.14.17.12.43-.05.58-.53.49-.7 1.26-.43 1.92.26.65.9 1.07 1.6 1.07.23 0 .48.16.5.38.07.59.07 1.18 0 1.77zm-5.1-.89a2.6 2.6 0 1 0-5.2 0 2.6 2.6 0 0 0 5.2 0z"></path></defs><g><g transform="translate(-1061 -163)"><use fill="#afb7bf" xlink:href="#75lva"></use></g></g></svg><span>Настройки</span></a></li>
								<li><a href="/user/sign-in/logout" data-method="post"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="14" height="14" viewBox="0 0 14 14"><defs><path id="dqcpa" d="M1063 205h7v2h-5v10h5v2h-7zm3 8v-2h7l-1.78-2.38 1.56-1.24 2.93 3.9 1.29.92-4.32 4.8-1.68-1.2 2.52-2.8z"></path></defs><g><g transform="translate(-1063 -205)"><use fill="#afb7bf" xlink:href="#dqcpa"></use></g></g></svg><span>Выйти</span></a></li>
							</ul>
						</li>
						<?}?>
					</ul>
						<?	if(Yii::$app->user->isGuest){}else{?>	
					<div class="panel__mobile-alerts row i-mid between hidden-lg hidden-md hidden-sm">
						<div class="panel__cart lk-link">
							<div class="lk-link__icon"><a href=""><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24" height="22" viewBox="0 0 24 22"><defs><path id="f7q1aasdasd" d="M1028 17l1 2h17c.69 0 1.47.28 1.47.97 0 .23 0 .34-.22.57L1043.1 29c-.34.69-1.2 1-2.11 1h-8l-1.36 2.1v.11c0 .12.25-.21.36-.21h13v2h-14c-1.26 0-1.88-.3-1.88-1.56 0-.34.11-.8.22-1.15l1.61-2.75L1027 19h-3v-2zm4 18a2 2 0 1 1 0 4 2 2 0 0 1 0-4zm10 0a2 2 0 1 1 0 4 2 2 0 0 1 0-4z"/><clipPath id="f7q1bs"><use fill="#fff" xlink:href="#f7q1aasdasd"/></clipPath></defs><g><g transform="translate(-1024 -17)"><use fill="#fff" fill-opacity="0" stroke="#000" stroke-miterlimit="50" stroke-width="4" clip-path="url(&quot;#f7q1bs&quot;)" xlink:href="#f7q1aasdasd"/></g></g></svg><span>4</span></a></div>
						</div>
						<div class="panel__alerts lk-link lk-link_has-hidden">
							<div class="lk-link__icon"><a href="buyer-alerts.html"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="22" height="23" viewBox="0 0 22 23"><defs><path id="z2yda" d="M1079 37h4c0 1.37-.63 2-2 2s-2-.63-2-2zm13-2v1h-22v-1.3l3-1.7v-8c0-3.86 3.38-6.13 7-7v-1c0-1 0-1 1-1s1 0 1 1v1c3.62.87 7 3.14 7 7v8zm-5-10c0-3.12-2.88-5-6-5s-6 1.88-6 5v9h12z"/></defs><g><g transform="translate(-1070 -16)"><use xlink:href="#z2yda"/></g></g></svg><span>12</span></a></div>
							<div class="lk-link__wrap">
								<div class="lk-link__wrap-title">уведомления</div>
								<div class="lk-link__wrap-load"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="14" height="14" viewBox="0 0 14 14"><g><g transform="translate(-1123 -110)"><image width="14" height="14" transform="translate(1123 110)" xlink:href="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAA4AAAAOCAYAAAAfSC3RAAABe0lEQVQ4T5WSvUvDUBTFz0vSJrHEpEhtanFzER2cHATt4NbFSXH03/A/EVzERQQHBymIDm4ibiLoIE5CjCUxSdPafJAnL9XaSuPH3d5793fe5dxDKKUUA+U4DoIgQBiG6W0+n4coitA0bbANhIF+TOEHMQLbRFEdhyAIKcCKCURRBN/3US6X0zdWJEoo3X0MsPPUxtGCghk1BzKk3TswAcuyUCqVUpjcugmdP/OALo/9moz1Cg+JH0F+wM1mE9VqFaR+6dLGAwdMC3hdEqHlRv33JeR5HpIkAcGJQ2HxuKiPYXmCA/czh06ng1arBYIDlyLgYG4WMCn+QgGI4xiGYYDg0KXocjA2CtCl/4CN3qjHqzLWKhmuDHjVH3Xr2qV79xwwxeOtJmc6+sm6rguWGXLnJXT23AO4HK5WZCwWs8dlu+yvox1Tevoc4ubFw/bceD8x3zfJINu20wDwPN+LHGtibpmmCUVRMiOn63oKpZH7S8glSYKqqkNDvAONtMA/jJk1eQAAAABJRU5ErkJggg=="/></g></g></svg></div>
								<div class="lk-link__wrap-list">
									<div class="lk-link__wrap-item">
										<div class="lk-link__wrap-text">Клиент заказал вашу деталь №3425483 и хочет заказать доставку на дом и оплатить наличными</div>
									</div>
									<div class="lk-link__wrap-item">
										<div class="lk-link__wrap-text">Клиент заказал вашу деталь №3425483 и хочет заказать доставку на дом и оплатить наличными</div>
									</div>
									<div class="lk-link__wrap-item">
										<div class="lk-link__wrap-text">Клиент заказал вашу деталь №3425483 и хочет заказать доставку на дом и оплатить наличными</div>
									</div>
									<div class="lk-link__wrap-item">
										<div class="lk-link__wrap-text">Клиент заказал вашу деталь №3425483 и хочет заказать доставку на дом и оплатить наличными</div>
									</div>
								</div>
								<a href="" class="lk-link__wrap-all">смотреть все уведомления</a>
							</div>
						</div>
						<div class="panel__messages lk-link lk-link_has-hidden">
							<div class="lk-link__icon"><a href="buyer-messages.html"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="22" height="17" viewBox="0 0 22 17"><defs><path id="oh16a" d="M1142 19v17h-22V19zm-3 2h-16l8 7.06zm-17 2v10l6-5zm2 11h14l-5-5-2 1-2-1zm16-1V23l-6 5z"/></defs><g><g transform="translate(-1120 -19)"><use xlink:href="#oh16a"/></g></g></svg><span>9</span></a></div>
							<div class="lk-link__wrap">
								<div class="lk-link__wrap-title">сообщения</div>
								<div class="lk-link__wrap-load"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="14" height="14" viewBox="0 0 14 14"><g><g transform="translate(-1123 -110)"><image width="14" height="14" transform="translate(1123 110)" xlink:href="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAA4AAAAOCAYAAAAfSC3RAAABe0lEQVQ4T5WSvUvDUBTFz0vSJrHEpEhtanFzER2cHATt4NbFSXH03/A/EVzERQQHBymIDm4ibiLoIE5CjCUxSdPafJAnL9XaSuPH3d5793fe5dxDKKUUA+U4DoIgQBiG6W0+n4coitA0bbANhIF+TOEHMQLbRFEdhyAIKcCKCURRBN/3US6X0zdWJEoo3X0MsPPUxtGCghk1BzKk3TswAcuyUCqVUpjcugmdP/OALo/9moz1Cg+JH0F+wM1mE9VqFaR+6dLGAwdMC3hdEqHlRv33JeR5HpIkAcGJQ2HxuKiPYXmCA/czh06ng1arBYIDlyLgYG4WMCn+QgGI4xiGYYDg0KXocjA2CtCl/4CN3qjHqzLWKhmuDHjVH3Xr2qV79xwwxeOtJmc6+sm6rguWGXLnJXT23AO4HK5WZCwWs8dlu+yvox1Tevoc4ubFw/bceD8x3zfJINu20wDwPN+LHGtibpmmCUVRMiOn63oKpZH7S8glSYKqqkNDvAONtMA/jJk1eQAAAABJRU5ErkJggg=="/></g></g></svg></div>
								<div class="lk-link__wrap-list">
									<div class="lk-link__wrap-item">
										<div class="lk-link__wrap-name">Антон Викторчук</div>
										<div class="lk-link__wrap-text lk-link__wrap-text_small">Fragile home appliences, all packed</div>
									</div>
									<div class="lk-link__wrap-item">
										<div class="lk-link__wrap-name">Антон Викторчук</div>
										<div class="lk-link__wrap-text lk-link__wrap-text_small">Fragile home appliences, all packed</div>
									</div>
									<div class="lk-link__wrap-item">
										<div class="lk-link__wrap-name">Антон Викторчук</div>
										<div class="lk-link__wrap-text lk-link__wrap-text_small">Fragile home appliences, all packed</div>
									</div>
									<div class="lk-link__wrap-item">
										<div class="lk-link__wrap-name">Антон Викторчук</div>
										<div class="lk-link__wrap-text lk-link__wrap-text_small">Fragile home appliences, all packed</div>
									</div>
									<div class="lk-link__wrap-item">
										<div class="lk-link__wrap-name">Антон Викторчук</div>
										<div class="lk-link__wrap-text lk-link__wrap-text_small">Fragile home appliences, all packed</div>
									</div>
								</div>
								<a href="" class="lk-link__wrap-all">смотреть все сообщения</a>
							</div>
						</div>
					</div>
					<?php } ?>
				</div>
			</div>
		</div>
	</div>
	<div class="search-panel">
		<div class="case">
			<div class="search-panel__wrap row i-mid between">
				<div class="<?	if(Yii::$app->user->isGuest){?>search search_lk<?}else{?>search search_full<?}?> ">
					<div class="search__icon"><svg id="SVGDoc" width="23" height="23" xmlns="http://www.w3.org/2000/svg" version="1.1" xmlns:xlink="http://www.w3.org/1999/xlink" xmlns:avocode="https://avocode.com/" viewBox="0 0 23 23"><defs><path d="M171.92848,110.56483c-0.22333,0.21613 -0.51713,0.33486 -0.82715,0.33486c-0.32744,0 -0.633,-0.13077 -0.86011,-0.36812l-5.43489,-5.6864c-1.54332,1.09088 -3.34834,1.66559 -5.24254,1.66559c-5.0453,0 -9.15032,-4.12953 -9.15032,-9.2051c0,-5.07581 4.10502,-9.20534 9.15032,-9.20534c5.04556,0 9.15041,4.12953 9.15041,9.20534c0,2.17235 -0.75986,4.25923 -2.14671,5.91779l5.39394,5.64361c0.45628,0.47743 0.44126,1.2387 -0.03296,1.69778zM152.80051,97.30566c0,3.75164 3.03394,6.80387 6.76329,6.80387c3.72934,0 6.76337,-3.05223 6.76337,-6.80387c0,-3.75187 -3.03403,-6.80411 -6.76337,-6.80411c-3.72934,0 -6.76329,3.05223 -6.76329,6.80411z" id="Path-0"/></defs><desc>Generated with Avocode.</desc><g transform="matrix(1,0,0,1,-150,-88)"><g><title>Forma 1</title><use xlink:href="#Path-0" fill="#7b7d80" fill-opacity="1"/></g></g></svg></div>
					<input type="text" class="search__input" placeholder="Поиск детали...">
				</div>
					<?	if(Yii::$app->user->isGuest){?>
				<a data-fancybox data-src="#popup-auth" href="#" class="search-panel__btn-lk btn btn_bg-transparent btn_border-transparent btn_color-black btn_normal"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="14" height="20" viewBox="0 0 14 20" class="icon"><defs><path id="obtya" d="M1092 96c0-2.54.77-5 4-5s3.8 2.66 3.8 5.2-1.7 4.8-3.8 4.8c-2.1 0-4-2.46-4-5zm-3.15 11.81c0-.04 0-.15 0 0zm14.32.13c0-.3 0-.05 0 0zM1103 109c0 .02-.92 2-7 2-5.46 0-6.75-1.6-6.96-1.94.04.22.1.66-.04-.06.26 1.29 0 .35 0 0v-.01c0-.05.02 0 .04.07l-.04-.06v-.01c.08-4.3.63-7.2 5-7.99 0 0 .55 1 2 1s2-1 2-1c4.42.8 4.93 3.58 5 8 0-.04 0-.1 0 0-.27 1.38 0 .04 0 0zm0 0z"/></defs><g><g transform="translate(-1089 -91)"><use xlink:href="#obtya"/></g></g></svg><span>Войти в Личный кабинет</span></a>
				<?}?>
			</div>
		</div>
	</div>
</header>

    <?php echo $content ?>
  
            	</div>
		<footer>
				<div class="page-content clearfix">
						<div class="footer-left pull-left">
								<p>© 2017  НеликвидаНет</p>
								<p>Все права защищены. Правила использования информации</p>
						</div>

				</div>
		</footer>
<div class="popup popup_auth block block_auth" id="popup-auth">

		
		<div class="popup__form">
			<div class="popup__title">войти</div>
			<div class="popup__form-wrap row i-bottom">



  <br>
    	<div class="popup__case row i-strech">
    	

<?php  
echo $form = \Yii::$app->view->renderFile(Yii::getAlias('@frontend') . '/modules/user/views/sign-in/signup.php'); ?>

  </div>



		
			</div>
		</div>
	</div>

	<div class="popup popup_large block block_popup" id="popup-offer">
		<div class="popup__title">договор оферта</div>
		<div class="popup__text text maxh200">
			<h2>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Dolorum, quaerat.</h2>
			<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Tenetur sequi veniam odio velit illum quasi vero delectus laborum natus voluptatem ab reprehenderit earum cum eius sit minus rerum ducimus, enim libero! Animi dolorem nulla id. Numquam quaerat quae repellendus architecto saepe, doloremque nobis inventore magnam aspernatur eligendi? Maiores et repellendus ad suscipit soluta animi officiis molestiae commodi explicabo eligendi accusamus, atque! Aliquid temporibus delectus ipsam et illum? Ipsa mollitia, esse quia delectus saepe velit impedit beatae ducimus sint, fuga molestias. Quo voluptates cupiditate placeat commodi pariatur error, perspiciatis est eius vero, debitis, ad veritatis. Repudiandae nulla fugit non exercitationem, quas.</p>
			<h2>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Dolorum, quaerat.</h2>
			<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Tenetur sequi veniam odio velit illum quasi vero delectus laborum natus voluptatem ab reprehenderit earum cum eius sit minus rerum ducimus, enim libero! Animi dolorem nulla id. Numquam quaerat quae repellendus architecto saepe, doloremque nobis inventore magnam aspernatur eligendi? Maiores et repellendus ad suscipit soluta animi officiis molestiae commodi explicabo eligendi accusamus, atque! Aliquid temporibus delectus ipsam et illum? Ipsa mollitia, esse quia delectus saepe velit impedit beatae ducimus sint, fuga molestias. Quo voluptates cupiditate placeat commodi pariatur error, perspiciatis est eius vero, debitis, ad veritatis. Repudiandae nulla fugit non exercitationem, quas.</p>
			<h2>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Dolorum, quaerat.</h2>
			<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Tenetur sequi veniam odio velit illum quasi vero delectus laborum natus voluptatem ab reprehenderit earum cum eius sit minus rerum ducimus, enim libero! Animi dolorem nulla id. Numquam quaerat quae repellendus architecto saepe, doloremque nobis inventore magnam aspernatur eligendi? Maiores et repellendus ad suscipit soluta animi officiis molestiae commodi explicabo eligendi accusamus, atque! Aliquid temporibus delectus ipsam et illum? Ipsa mollitia, esse quia delectus saepe velit impedit beatae ducimus sint, fuga molestias. Quo voluptates cupiditate placeat commodi pariatur error, perspiciatis est eius vero, debitis, ad veritatis. Repudiandae nulla fugit non exercitationem, quas.</p>
			<h2>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Dolorum, quaerat.</h2>
			<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Tenetur sequi veniam odio velit illum quasi vero delectus laborum natus voluptatem ab reprehenderit earum cum eius sit minus rerum ducimus, enim libero! Animi dolorem nulla id. Numquam quaerat quae repellendus architecto saepe, doloremque nobis inventore magnam aspernatur eligendi? Maiores et repellendus ad suscipit soluta animi officiis molestiae commodi explicabo eligendi accusamus, atque! Aliquid temporibus delectus ipsam et illum? Ipsa mollitia, esse quia delectus saepe velit impedit beatae ducimus sint, fuga molestias. Quo voluptates cupiditate placeat commodi pariatur error, perspiciatis est eius vero, debitis, ad veritatis. Repudiandae nulla fugit non exercitationem, quas.</p>
		</div>
	</div>
	<div class="popup popup_large block block_popup" id="popup-policy">
		<div class="popup__title">политика конфиденциальности</div>
		<div class="popup__text text maxh200">
			<h2>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Dolorum, quaerat.</h2>
			<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Tenetur sequi veniam odio velit illum quasi vero delectus laborum natus voluptatem ab reprehenderit earum cum eius sit minus rerum ducimus, enim libero! Animi dolorem nulla id. Numquam quaerat quae repellendus architecto saepe, doloremque nobis inventore magnam aspernatur eligendi? Maiores et repellendus ad suscipit soluta animi officiis molestiae commodi explicabo eligendi accusamus, atque! Aliquid temporibus delectus ipsam et illum? Ipsa mollitia, esse quia delectus saepe velit impedit beatae ducimus sint, fuga molestias. Quo voluptates cupiditate placeat commodi pariatur error, perspiciatis est eius vero, debitis, ad veritatis. Repudiandae nulla fugit non exercitationem, quas.</p>
			<h2>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Dolorum, quaerat.</h2>
			<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Tenetur sequi veniam odio velit illum quasi vero delectus laborum natus voluptatem ab reprehenderit earum cum eius sit minus rerum ducimus, enim libero! Animi dolorem nulla id. Numquam quaerat quae repellendus architecto saepe, doloremque nobis inventore magnam aspernatur eligendi? Maiores et repellendus ad suscipit soluta animi officiis molestiae commodi explicabo eligendi accusamus, atque! Aliquid temporibus delectus ipsam et illum? Ipsa mollitia, esse quia delectus saepe velit impedit beatae ducimus sint, fuga molestias. Quo voluptates cupiditate placeat commodi pariatur error, perspiciatis est eius vero, debitis, ad veritatis. Repudiandae nulla fugit non exercitationem, quas.</p>
			<h2>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Dolorum, quaerat.</h2>
			<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Tenetur sequi veniam odio velit illum quasi vero delectus laborum natus voluptatem ab reprehenderit earum cum eius sit minus rerum ducimus, enim libero! Animi dolorem nulla id. Numquam quaerat quae repellendus architecto saepe, doloremque nobis inventore magnam aspernatur eligendi? Maiores et repellendus ad suscipit soluta animi officiis molestiae commodi explicabo eligendi accusamus, atque! Aliquid temporibus delectus ipsam et illum? Ipsa mollitia, esse quia delectus saepe velit impedit beatae ducimus sint, fuga molestias. Quo voluptates cupiditate placeat commodi pariatur error, perspiciatis est eius vero, debitis, ad veritatis. Repudiandae nulla fugit non exercitationem, quas.</p>
			<h2>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Dolorum, quaerat.</h2>
			<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Tenetur sequi veniam odio velit illum quasi vero delectus laborum natus voluptatem ab reprehenderit earum cum eius sit minus rerum ducimus, enim libero! Animi dolorem nulla id. Numquam quaerat quae repellendus architecto saepe, doloremque nobis inventore magnam aspernatur eligendi? Maiores et repellendus ad suscipit soluta animi officiis molestiae commodi explicabo eligendi accusamus, atque! Aliquid temporibus delectus ipsam et illum? Ipsa mollitia, esse quia delectus saepe velit impedit beatae ducimus sint, fuga molestias. Quo voluptates cupiditate placeat commodi pariatur error, perspiciatis est eius vero, debitis, ad veritatis. Repudiandae nulla fugit non exercitationem, quas.</p>
		</div>
	</div>
<!--	  <?php
    NavBar::begin([
        'options' => [
            'class' => 'panel__wrap row i-mid',
        ],
    ]); ?>
    <?php echo Nav::widget([
        'options' => ['class' => 'panel__persona persona hidden-md hidden-sm hidden-xs'],
        'items' => [
            ['label' => Yii::t('frontend', 'Signup'), 'url' => ['/user/sign-in/signup'], 'visible'=>Yii::$app->user->isGuest],
            ['label' => Yii::t('frontend', 'Login'), 'url' => ['/user/sign-in/login'], 'visible'=>Yii::$app->user->isGuest],
            [
                'label' => Yii::$app->user->isGuest ? '' : Yii::$app->user->identity->getPublicIdentity(),
                'visible'=>!Yii::$app->user->isGuest,
                'items'=>[
                    [
                        'label' => Yii::t('frontend', 'Settings'),
                        'url' => ['/user/default/index']
                    ],
                    [
                        'label' => Yii::t('frontend', 'Backend'),
                        'url' => Yii::getAlias('@backendUrl'),
                        'visible'=>Yii::$app->user->can('manager')
                    ],
                    [
                        'label' => Yii::t('frontend', 'Logout'),
                        'url' => ['/user/sign-in/logout'],
                        'linkOptions' => ['data-method' => 'post']
                    ]
                ]
            ],
            [
                'label'=>Yii::t('frontend', 'Language'),
                'items'=>array_map(function ($code) {
                    return [
                        'label' => Yii::$app->params['availableLocales'][$code],
                        'url' => ['/site/set-locale', 'locale'=>$code],
                        'active' => Yii::$app->language === $code
                    ];
                }, array_keys(Yii::$app->params['availableLocales']))
            ]
        ]
    ]); ?>

    <?php NavBar::end(); ?>   -->

    <script type="text/javascript">
 
  jQuery(function($){
	$(".scroll-block").on("click","a", function (event) {
		//отменяем стандартную обработку нажатия по ссылке
		event.preventDefault();

		//забираем идентификатор бока с атрибута href
		var id  = $(this).attr('href'),

		//узнаем высоту от начала страницы до блока на который ссылается якорь
			top = $(id).offset().top;
		
		//анимируем переход на расстояние - top за 1500 мс
		$('body,html').animate({scrollTop: top}, 1500);
	});
	
/*	$(document).ready(function() {
	$(".call").on("keyup", function() {
  	var value = $(this).val();
  	$(this).val($(this).data("initial") + value.substring(3));
  });
});*/




  

  $(".call").inputmask({"mask": "+7 (999) 999-99-99"}); //specifying options

  
     // $(".call").focus();
     // $(".call").get(0).setSelectionRange(0,0);
      
       
      });
      
       
   </script> 
    <? if(Yii::$app->user->isGuest){
    $id = '';	
    }else{?>
   <? 
      $id = \Yii::$app->user->identity->id;
   ?>
   <!-- Автоматическая проверка наличия сообщений и разблокировка глобального счетчика. При работе на странице чата так-же вызов цикла подгрузки сообщений-->
    <script>
        // данная переменная должна формироваться на сервере и содержать числовой идентификатор пользователя в системе (id)
        var loggedinUserId = <?=$id?>;        
        var processRefresh = true;
        var totalMessagesSpan = $('#totalUnreadMessages');
        var totalMessagesSpanRc = $('#totalUnreadMessagesRc');
        var showRigthChatPanel = false;
    </script>  

<?
	$id = \Yii::$app->user->identity->id;
		$sc = 'asfghjkcvegdghjer';

		$url = 'http://web.nelikvida-net.ru/api/v1/Account/getservertoken/' . $id . '/' . $sc;

		$client = new Client();
		$response = $client->createRequest()
			->setUrl($url)
			->setMethod('post')
			->setData('')
			->send();

?>      
<script>
sessionStorage.setItem('accessToken', '<?=$response->content?>');
</script>
<?}?>



<!-- right-chat-toggle-->
<input type="checkbox" id="right-chat-toggle" hidden>
<div class="mask-content"></div>
<script>
    // var elem = document.createElement('script');
    // elem.src = "/viewbox/jquery.viewbox.min.js";
    // document.body.appendChild(elem);
</script>
 <nav class="nav" id="rigth-chat-app">

        <div class="nav-header">
            <h2 v-if="!showMessagePanel">
                ЧАТ ПОДДЕРЖКИ
                <span class="rigth_chat_count_bg" v-if="unreadMessages > 0 && !showMessagePanel">
                    {{unreadMessages}}
                </span>
                <!-- maximize -->
                <span class="close_span maximize_btn" @click="collapse" v-if="application.state === application.stateMin"></span>
                <!-- collapse -->
                <span class="close_span restore_btn" @click="maximize" v-if="application.state === application.stateMax"></span>
                <!-- minimize  -->
                <span class="close_span minimize_btn" @click="closePanel"></span>
            </h2>
            <h2 v-else>
                <span class="back_span back_btn" @click="backToTopics" style="cursor: pointer;"></span>
                <span @click="backToTopics" style="cursor: pointer;">ВЫЙТИ ИЗ ДИАЛОГА</span>
                <!-- maximize -->
                <span class="close_span maximize_btn" @click="collapse" v-if="application.state === application.stateMin"></span>
                <!-- collapse -->
                <span class="close_span restore_btn" @click="maximize" v-if="application.state === application.stateMax"></span>
                <!-- minimize  -->
                <span class="close_span minimize_btn" @click="closePanel"></span>

            </h2>
        </div>
        <input type="text"
               placeholder="Поиск..."
               class="rigth_chat_search_input"
               v-model="query"
               @input="search"
               v-if="!showMessagePanel" />
        <div v-if="showMessagePanel" class="anonse_bar">

            <div class="row">
                <div class="col-md-2 mp-0">
                    <img class="w48r3" :src="'/api/v1/article/' + actualtopic.id + '/img'">
                </div>
                <div class="col-md-10 mp-0">
                    <div class="anonse_header">
                        {{actualtopic.title}} <br/>
                        Артикул: {{actualtopic.vendorCode}}<br />
                        Производитель: {{actualtopic.vendor}} <br />
                        Цена: {{ actualtopic.price }} <img src='/img/rc/rub13.png' style="height: 11px"/>
                        <a :href="'/article/view?id=' + actualtopic.id">к обьявлению </a>
                    </div>
                </div>

            </div>
        </div>
        <transition name="fade" >
            <div class="ms_container" v-show="!showMessagePanel">
                <template class="">
                    <a v-for="topic in topics"
                       :href="'#message-' + topic.id"
                       v-bind:class="{active : topic.selected, active: topic.hasMessages}"
                       class="msg_container mp-0 h100"
                       v-bind:class="{ selected: topic.selected }"
                       @click="showThread(topic.id, this, $event)">

                        <div class="row h68">
                            <div class="col-md-2 mp-0 ms_first">
                                <img :src="'/api/v1/user/avatar/' + topic.authorId" class="w48 " />
                            </div>
                            <div class="col-md-8 mp-0 ms_body">
                                <span v-bind:class="{'message-name-active':  topic.hasMessages}"
                                      class="message-name">
                                    {{ topic.lmName }}
                                </span>
                                <span v-bind:class="{ readed : topic.lmIsReaded }"
                                      class=" message-time-label">
                                    {{topic.lmCreated | formatMonthDayEx }}
                                </span>                                
                                <br />

                                <span class="message-text" v-if="topic.lmIsCurrent">
                                    Вы: {{topic.lastMessage |striphtml}}
                                </span>
                                <span class="message-text" v-else>
                                    {{topic.lastMessage |striphtml}}
                                </span>
                            </div>
                            <div class="col-md-2 mp-0 end_col">
                                <img class="w48r3" :src="'/api/v1/article/' + topic.announcementId + '/img'">
                            </div>
                        </div>
                        <div class="row low_row_border">
                            <div class="col-md-12 mp-0 message-meta">
                                {{topic.title}} Цена: {{ topic.price }} <img src="/img/rc/run14it.png" style="height: 11px"/> <br />
                                Артикул: {{topic.vendorCode}} Производитель: {{topic.vendor}}
                            </div>
                        </div>
                    </a>
                    <a v-if="topics.length == 0">
                        <div class="no_result">
                            Нет результатов
                        </div>
                    </a>
                </template>
            </div>
            
        </transition>      
        <transition name="unfade" >
            <div class="ms_container" v-if="showMessagePanel">
                <div v-if="showMessagePanel">
                    <div v-if="posts.length == 0">
                        В данном чате пока нет сообщений.
                    </div>
                    <div v-for="msg in posts"
                         v-bind:class="{ message_to: !msg.isAuthor, message_from: msg.isAuthor}"
                         class="row">

                        <div class="col-md-12 day_repeater" v-if="msg.newDay">
                            {{msg.created | formatMonthDay }}
                        </div>

                        <div class="col-md-2 mp-0 avatar_flex">
                            <img :src="msg.avatarUrl"
                                 alt=""
                                 class="w40 mlt-5"
                                 v-if="!msg.isAuthor">
                        </div>
                        <div class="col-md-10 mp-0" v-bind:class="{fac_r: msg.isAuthor}">
                            <div v-if="!msg.isAuthor" class="top_name">{{msg.name}}</div>
                            <div v-bind:class="{ from_alien: !msg.isAuthor, from_author: msg.isAuthor }">
                                <div v-html="msg.body" class="mgs_body"></div>
                                <template v-if="msg.attachment && msg.attachment.name != null">
                                    <br />
                                    <a :href="msg.attachment.url" onclick="//return jsiBoxOpen(this)" target="_self" class="litebox" data-litebox-group="group-1" tabindex="0">
                                        <img class="img-responsive"
                                             style="width: 250px !important"
                                             v-if="msg.attachment.isImage"
                                             :src="msg.attachment.url"
                                             :alt="msg.attachment.name" />
                                    </a>
                                    <b>
                                        <a :href="msg.attachment.url" class="attachment_url" target="_blank">
                                            <div class="row" v-if="!msg.attachment.isImage">
                                                <div class="col-md-2 mp-0">
                                                    <img v-if="!msg.attachment.isImage"
                                                         :src="msg.attachment.name | getIcon"
                                                         class="document_ico" />
                                                </div>
                                                <div class="col-md-10 mp-0 mgs_file" :title="msg.attachment.name">
                                                    {{msg.attachment.name}}
                                                    <div class="download_size">{{msg.attachment.size | fileSize}}</div>
                                                </div>
                                            </div>
                                        </a>
                                    </b>
                                </template>


                                <div v-bind:class="{  tl_right: true, tl_readed: msg.isRead &&  msg.isAuthor, tl_not_readed: !msg.isRead &&  msg.isAuthor }"
                                     :data-created="msg.created"
                                     class="time_label tl_short">
                                    {{ msg.created | formatTime }}
                                </div>
                                <div v-bind:class="{ tl_right: true, tl_readed: msg.isRead &&  msg.isAuthor, tl_not_readed: !msg.isRead &&  msg.isAuthor }"
                                     :data-created="msg.created"
                                     class="time_label tl_long">
                                    {{ msg.created | formatDateTime }}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </transition>
        <!-- send message bar-->
        <div class="send_panel" v-if="showMessagePanel" id="send_panel">
            <div class="loader" v-if="showLoader"></div>
            <div class="row" style="height: 100%">
                <div class="messager__file file file_small col-md-2 mp-0 flx_bt">
                    <form>
                        <label for="file-rch">
                            <img src="/img/rc/attachment.png" title="selectFileName" style="margin-left: 20px; margin-bottom: 11px;" />
                        </label>
                        <input type="file" id="file-rch" name="uploads" @change="fileSelected">
                    </form>
                </div>
                <div class="col-md-8 mp-0">
                    <textarea class="send_ta" id="ta_send_panel"
                                  placeholder="Напишите сообщение..."
                                  v-model="messageArea"
                                  @keyup="resizeSendArea"
                                  @keyup.enter.exact="sendMessage"
                                  @keyup.enter.ctrl.exact="appendNewLine"></textarea>
                </div>
                <div class="col-md-2 mp-0 flx_bt">
                    <img src="/img/rc/send.png" width="23px" class="" @click="sendMessage" title="Отправить" style="margin-bottom: 17px;margin-left: 7px; " />
                </div>
            </div>
        </div>
    </nav>
<?	if(Yii::$app->user->isGuest){}else{?>  
 <label for="right-chat-toggle" class="nav-toggle float_button_no_bg" onclick>                
                <span id="totalUnreadMessagesRc" style="display: none;"></span>
            </label>
   <?php } ?>         
   
<? if(Yii::$app->user->isGuest){    
    }else{?>
    <script src="/js/rigth-chat.js"></script>
   <?   
}    
   ?>

<script>$('.litebox2').viewbox();</script>
<script>$('.litebox').viewbox();</script>
<?$url = strtok(str_replace('http://'.$_SERVER['SERVER_NAME'], "", yii\helpers\Url::current([], true)), '?');?>
<?if($url == '/site/index'){?>
<script> showRigthChat(false); </script>
<?}else{?>
<script> showRigthChat(true); </script>
<?}?>
 
</body>
</html>
<?php $this->endContent() ?>
