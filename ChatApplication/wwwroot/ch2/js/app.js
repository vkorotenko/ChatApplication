(function(t){function e(e){for(var s,o,r=e[0],c=e[1],l=e[2],m=0,d=[];m<r.length;m++)o=r[m],n[o]&&d.push(n[o][0]),n[o]=0;for(s in c)Object.prototype.hasOwnProperty.call(c,s)&&(t[s]=c[s]);u&&u(e);while(d.length)d.shift()();return i.push.apply(i,l||[]),a()}function a(){for(var t,e=0;e<i.length;e++){for(var a=i[e],s=!0,r=1;r<a.length;r++){var c=a[r];0!==n[c]&&(s=!1)}s&&(i.splice(e--,1),t=o(o.s=a[0]))}return t}var s={},n={app:0},i=[];function o(e){if(s[e])return s[e].exports;var a=s[e]={i:e,l:!1,exports:{}};return t[e].call(a.exports,a,a.exports,o),a.l=!0,a.exports}o.m=t,o.c=s,o.d=function(t,e,a){o.o(t,e)||Object.defineProperty(t,e,{enumerable:!0,get:a})},o.r=function(t){"undefined"!==typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(t,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(t,"__esModule",{value:!0})},o.t=function(t,e){if(1&e&&(t=o(t)),8&e)return t;if(4&e&&"object"===typeof t&&t&&t.__esModule)return t;var a=Object.create(null);if(o.r(a),Object.defineProperty(a,"default",{enumerable:!0,value:t}),2&e&&"string"!=typeof t)for(var s in t)o.d(a,s,function(e){return t[e]}.bind(null,s));return a},o.n=function(t){var e=t&&t.__esModule?function(){return t["default"]}:function(){return t};return o.d(e,"a",e),e},o.o=function(t,e){return Object.prototype.hasOwnProperty.call(t,e)},o.p="/";var r=window["webpackJsonp"]=window["webpackJsonp"]||[],c=r.push.bind(r);r.push=e,r=r.slice();for(var l=0;l<r.length;l++)e(r[l]);var u=c;i.push([0,"chunk-vendors"]),a()})({0:function(t,e,a){t.exports=a("56d7")},"0e36":function(t,e,a){},"2e82":function(t,e,a){"use strict";var s=a("8ffb"),n=a.n(s);n.a},"56d7":function(t,e,a){"use strict";a.r(e);a("cadf"),a("551c"),a("f751"),a("097d");var s=a("2b0e"),n=function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("nav",{staticClass:"nav",attrs:{id:"rigth-chat-app"}},[a("NavHeader"),a("router-view")],1)},i=[],o=function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("div",{staticClass:"nav-header"},[a("div",{staticClass:"row btn_bar"},[a("div",{staticClass:"col-md-2 mp-0 w36"},[a("ul",{staticClass:"ul_btn_bar"},[t.showBackButton?a("li",{on:{click:t.backToTopics}},[a("img",{staticClass:"back_span back_btn",attrs:{src:"/img/rc/back_ico.png"}})]):t._e()])]),a("div",{staticClass:"col-md-8 mp-0",staticStyle:{"padding-top":"9px"}},[a("h2",[t._v("\n                "+t._s(t.titleText)+"\n            ")]),t.unreadMessages>0&&!t.messageMode?a("span",{staticClass:"rigth_chat_count_bg"},[t._v("\n                "+t._s(t.unreadMessages)+"\n            ")]):t._e()]),a("div",{staticClass:"col-md-2 mp-0",staticStyle:{width:"74px","margin-left":"auto"}},[a("ul",{staticClass:"ul_btn_bar"},[a("li",{on:{click:t.closePanel}},[a("span",{staticClass:"close_span minimize_btn"})]),t.appstatemax?a("li",{on:{click:t.collapse}},[t.appstatemax?a("span",{staticClass:"close_span restore_btn"}):t._e()]):t._e(),t.appstatemax?t._e():a("li",{on:{click:t.maximize}},[t.appstatemax?t._e():a("span",{staticClass:"close_span maximize_btn"})])])])])])},r=[],c=a("cebc"),l=a("2f62"),u="openPanelKey",m={name:"NavHeader",props:{title:{type:String,default:"ЧАТ ПОДДЕРЖКИ"}},data:function(){return{messageMode:!1,titleText:this.title,showBackButton:!1,unreadMessages:0}},computed:{appstatemax:function(){return this.$store.state.appstatemax}},methods:Object(c["a"])({},Object(l["b"])({minimizeChat:"minimize",maximizeChat:"maximize"}),{switchTitle:function(){this.messageMode=!this.messageMode,this.messageMode?(this.titleText="ВЫЙТИ ИЗ ДИАЛОГА",this.showBackButton=!0):(this.titleText="ЧАТ ПОДДЕРЖКИ",this.showBackButton=!1)},backToTopics:function(){window.console.log("backToTopics")},closePanel:function(){var t=getComputedStyle(document.getElementById("rigth-chat-app")).bottom,e=!1;"0px"==t?window.console.log("bottom: 0"):window.console.log("bottom: "+t),window.console.log("closeRigthPanel: "+e),localStorage.setItem(u,e),showRigthChat(e)},collapse:function(){window.console.log("minimize");var t=document.getElementById("rigth-chat-app");t.style.transition="",setTimeout(function(){t.style.transition="opacity .4s ease-in, height 0.8s ease-in-out ",t.style.height="60%"},4),setTimeout(function(){t.style.transition=""},800),this.minimizeChat()},maximize:function(){window.console.log("maximize");var t=document.getElementById("rigth-chat-app");t.style.transition="",setTimeout(function(){t.style.transition="opacity .4s ease-in, height 0.8s ease-in-out ",t.style.height="100%"},4),setTimeout(function(){t.style.transition=""},800),this.maximizeChat()}})},d=m,p=a("2877"),h=Object(p["a"])(d,o,r,!1,null,"0fbb9469",null),g=h.exports,f={name:"App",components:{NavHeader:g}},_=f,v=Object(p["a"])(_,n,i,!1,null,null,null),b=v.exports,y=function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("label",{class:{float_button_no_bg:0==t.count,float_button_yes_bg:t.count>0,"nav-toggle":!0},attrs:{for:"right-chat-toggle",onclick:""}},[t.count>0?a("span",[t._v(t._s(t.count))]):t._e(),a("span",{staticStyle:{display:"none"},attrs:{id:"totalUnreadMessagesRc"}})])},w=[],C=(a("c5f6"),{name:"FloatButton",props:{unread:{type:Number,default:0}},data:function(){return{count:this.unread}},methods:{setCount:function(t){window.console.log(t),this.count=t}}}),x=C,k=Object(p["a"])(x,y,w,!1,null,null,null),S=k.exports,M=a("8c4f"),B=function(){var t=this,e=t.$createElement,a=t._self._c||e;return t.inthread?a("div",{staticClass:"fh_100"},[a("AnonseBar"),a("MessageList"),a("SendPanel")],1):a("div",{staticClass:"fh_100"},[a("SearchBox"),a("TopicsList")],1)},O=[],T=function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("input",{directives:[{name:"model",rawName:"v-model",value:t.query,expression:"query"}],staticClass:"rigth_chat_search_input",attrs:{type:"text",placeholder:"Поиск..."},domProps:{value:t.query},on:{input:function(e){e.target.composing||(t.query=e.target.value)}}})},j=[],E={name:"SearchBox",props:{query:String}},z=E,P=Object(p["a"])(z,T,j,!1,null,"e8931850",null),$=P.exports,I=function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("div",{staticClass:"ms_container first_hider"},[t._l(t.topics,function(e){return a("a",{staticClass:"msg_container mp-0 h100",class:{active:e.selected,active:e.hasMessages,selected:e.selected},attrs:{href:"#message-"+e.id},on:{click:function(a){return t.showThread(e.id,this,a)}}},[a("div",{staticClass:"row h68"},[a("div",{staticClass:"col-md-2 mp-0 ms_first"},[a("img",{staticClass:"w48 ",attrs:{src:"/api/v1/user/avatar/"+e.authorId}})]),a("div",{staticClass:"col-md-8 mp-0 ms_body"},[a("span",{staticClass:"message-name",class:{"message-name-active":e.hasMessages}},[t._v("\n                    "+t._s(e.lmName)+"\n                ")]),a("span",{staticClass:" message-time-label",class:{readed:e.lmIsReaded}},[t._v("\n                    "+t._s(t._f("formatMonthDayEx")(e.lmCreated))+"\n                ")]),a("br"),e.lmIsCurrent?a("span",{staticClass:"message-text"},[t.typingData&&t.typingData.topic==e.id?a("span",{staticClass:"body_typing_i"},[t._v("\n                        печатает\n                    ")]):[t._v("\n                        Вы: "+t._s(t._f("striphtml")(e.lastMessage))+"\n                    ")]],2):a("span",{staticClass:"message-text"},[t.typingData&&t.typingData.topic==e.id?a("span",{staticClass:"body_typing_i"},[t._v("\n                        печатает\n                    ")]):[t._v("\n                        "+t._s(t._f("striphtml")(e.lastMessage))+"\n                    ")]],2)]),a("div",{staticClass:"col-md-2 mp-0 end_col"},[a("img",{staticClass:"w48r3",attrs:{src:"/api/v1/article/"+e.announcementId+"/img"}})])]),a("div",{staticClass:"row low_row_border"},[a("div",{staticClass:"col-md-12 mp-0 message-meta"},[t._v("\n                "+t._s(e.title)+" Цена: "+t._s(e.price)+" "),a("img",{staticStyle:{height:"11px"},attrs:{src:"/img/rc/run14it.png"}}),a("br"),t._v("\n                Артикул: "+t._s(e.vendorCode)+" Производитель: "+t._s(e.vendor)+"\n            ")])])])}),0==t.topics.length?a("a",[a("div",{staticClass:"no_result"},[t._v("\n            Нет результатов\n        ")])]):t._e()],2)},A=[],N={name:"TopicsList",computed:{topics:function(){return window.console.log(this.$store.state.topics),this.$store.state.topics}}},K=N,L=Object(p["a"])(K,I,A,!1,null,"e6784ba8",null),D=L.exports,R=function(){var t=this,e=t.$createElement;t._self._c;return t._m(0)},q=[function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("div",{staticClass:"anonse_bar"},[a("div",{staticClass:"row"},[a("div",{staticClass:"col-md-2 mp-0"},[a("img",{staticClass:"w48r3",attrs:{src:"/api/v1/article/53/img"}})]),a("div",{staticClass:"col-md-10 mp-0"},[a("div",{staticClass:"anonse_header"},[t._v("\n                Капот для ВАЗ "),a("br"),t._v("\n                Артикул: Капот"),a("br"),t._v("\n                Производитель: Стрелка 11 "),a("br"),t._v("\n                Цена: 42\n                "),a("img",{staticStyle:{height:"11px"},attrs:{src:"/img/rc/rub13.png"}}),a("a",{attrs:{href:"/article/view?id=53"}},[t._v("к обьявлению ")])])])])])}],F={name:"AnonseBar",props:{}},H=F,J=Object(p["a"])(H,R,q,!1,null,"0eb21d89",null),U=J.exports,G=function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("div",{staticClass:" ms_container "})},Q=[],V={name:"MessageList",props:{}},W=V,X=Object(p["a"])(W,G,Q,!1,null,"4baddb1b",null),Y=X.exports,Z=function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("div",{staticClass:"send_panel",attrs:{id:"send_panel"}},[t.showLoader?a("div",{staticClass:"loader"}):t._e(),a("div",{staticClass:"row",staticStyle:{height:"100%"}},[a("div",{staticClass:"messager__file file file_small col-md-2 mp-0 flx_bt"},[a("form",[t._m(0),a("input",{attrs:{type:"file",id:"file-rch",name:"uploads"},on:{change:t.fileSelected}})])]),a("div",{staticClass:"col-md-8 mp-0"},[a("textarea",{directives:[{name:"model",rawName:"v-model",value:t.messageArea,expression:"messageArea"}],staticClass:"send_ta",attrs:{id:"ta_send_panel",placeholder:"Напишите сообщение..."},domProps:{value:t.messageArea},on:{keyup:[t.resizeSendArea,function(e){return!e.type.indexOf("key")&&t._k(e.keyCode,"enter",13,e.key,"Enter")?null:e.ctrlKey||e.shiftKey||e.altKey||e.metaKey?null:t.sendMessage(e)},function(e){return!e.type.indexOf("key")&&t._k(e.keyCode,"enter",13,e.key,"Enter")?null:e.ctrlKey?e.shiftKey||e.altKey||e.metaKey?null:t.appendNewLine(e):null}],input:function(e){e.target.composing||(t.messageArea=e.target.value)}}})]),a("div",{staticClass:"col-md-2 mp-0 flx_bt"},[a("img",{staticClass:"send_panel__btn",attrs:{src:"/img/rc/send.png",title:"Отправить"},on:{click:t.sendMessage}})])])])},tt=[function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("label",{attrs:{for:"file-rch"}},[a("img",{staticClass:"send_panel__attachment",attrs:{src:"/img/rc/attachment.png",title:"selectFileName"}})])}],et={name:"SendPanel",data:function(){return{messageArea:"",showLoader:!1}},props:{topicid:Number},methods:{resizeSendArea:function(){}}},at=et,st=(a("61ab"),Object(p["a"])(at,Z,tt,!1,null,"605a3adf",null)),nt=st.exports,it={name:"home",data:function(){return{inthread:!1,threadid:null}},components:{SearchBox:$,TopicsList:D,AnonseBar:U,MessageList:Y,SendPanel:nt},methods:{}},ot=it,rt=(a("2e82"),Object(p["a"])(ot,B,O,!1,null,"d21d8a74",null)),ct=rt.exports;s["a"].use(M["a"]);var lt=new M["a"]({routes:[{path:"/",name:"home",component:ct}]}),ut=(a("96cf"),a("3b8d"));s["a"].use(l["a"]);var mt=new l["a"].Store({state:{appstatemax:!0,closed:!1,topics:{},userid:null},mutations:{maximize:function(t){t.appstatemax=!0},minimize:function(t){t.appstatemax=!1},close:function(t){t.closed=!0},open:function(t){t.closed=!1}},actions:{actionA:function(){var t=Object(ut["a"])(regeneratorRuntime.mark(function t(e){var a;return regeneratorRuntime.wrap(function(t){while(1)switch(t.prev=t.next){case 0:return a=e.commit,t.t0=a,t.next=4,getData();case 4:t.t1=t.sent,(0,t.t0)("gotData",t.t1);case 6:case"end":return t.stop()}},t)}));function e(e){return t.apply(this,arguments)}return e}()}});s["a"].config.productionTip=!1;var dt="openPanelKey",pt=new s["a"]({router:lt,store:mt,render:function(t){return t(b)}});pt.$mount("#rigth-chat-app"),window.FloatMessageButton=new s["a"]({render:function(t){return t(S)}}),window.FloatMessageButton.$mount("#floatMessageButton"),window.showRigthChat=function(t,e){var a=localStorage.getItem(dt);null!=a&&(t=JSON.parse(a)),e&&(t=!0),document.getElementById("right-chat-toggle").checked=!!t&&"checked"},document.getElementById("right-chat-toggle").onchange=function(t){var e=document.getElementById("right-chat-toggle").checked;localStorage.setItem(dt,e)}},"61ab":function(t,e,a){"use strict";var s=a("0e36"),n=a.n(s);n.a},"8ffb":function(t,e,a){}});
//# sourceMappingURL=app.js.map