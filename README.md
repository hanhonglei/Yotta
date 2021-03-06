# Yotta

- More information, please feel free to visit my personal website [here](https://hanhonglei.github.io/).

- The execute application can be downloaded from [here](http://pan.baidu.com/s/1kVM3vwV).

2D tower defense game based on Unity
## 游戏简介
游戏名称：YOTTA（游塔）
游戏平台：面向PC平台与移动设备平台市场
目标用户：面向10岁以上的普通玩家，以及前往与YOTTA合作的景区旅游的游客

### 游戏内容：
YOTTA（游塔）是一款以景区旅游为主题的类塔防游戏。受旅游热潮的推动，希望将中国各地的旅游特色以娱乐的方式推广，希望能在宣传旅游景点的同时加深用户对该景区的认识，宣传景区知名度，从而带来收益。本游戏将塔防和模拟经营这两种游戏类型元素相结合，玩家既能够从游戏中体验到模拟经营元素中建筑摆放美感与功能资源搭配的趣味，同时也要尽力在关卡内所有游客走出景区前达成胜利通关的条件。
### 游戏特色：
在游戏具有高可玩性的基础上，能实现景区管理、旅游体验和城市规划等应用型功能需求。从游戏机制上，YOTTA的小关卡与一些真实的旅游区相对应，给玩家提供游戏与真实旅游区感性体验相结合的娱乐体验；不同于传统塔防游戏那种建筑向路径上的移动物体发射炮弹消灭敌人的模式，YOTTA运用的模式是：对不同种类的建筑拥有不同强度偏好的游客，按判定机制随行走主动进入路径边的建筑从而产出用于游戏核心循环运作的重要资源以推进游戏。从美术风格上，采用“拙”感卡通风格作为美术上的主导风格；从商业价值上，可以与特定景区合作，将景区内的景点加入到游戏关卡设施库中，玩家在建设这些虚拟景点时能够获得其对应的真实景点信息，从而加深用户对该景区的认知。
## 创意设计
### 游戏主题:
以旅游作为设计主题，以和旅游业发展结合作为主要的设计目的，以整个中国地图作为游戏整体背景，其中包含若干小关卡，分别是各个著名的旅游城市，当前已经实现的城市有北京和上海。当前已经实现的程序为单机版，后期实现运行于手机平台。
### 游戏规则:
(1)	玩家可在建造位置选择建造不同种类的建筑，如：商店，酒店，餐厅等。也可以选择升级已有建筑到更高等级。建造和升级建筑均需消耗金币，但更高等级的建筑可使游客产生更高的快乐度与金币消耗
(2)	游戏每个关卡中均存在固定数量的游客等候进入旅游区，游客种类包括：孩童，男士，女士，老人，外星人等
(3)	建筑物有影响范围，只有进入到影响范围内的游客才会参加偏好判定
(4)	当游客进入建筑物的影响范围时，间隔固定时间会对游客进行偏好判定，若成功则表明该游客进入该建筑物，建筑物产出金币并提升游客快乐度
(5)	游客进入场景时本身具有一定的初始快乐度，但其快乐度自其进入场景起，随时间递减
(6)	当游客的快乐度达到峰值时，将产出一枚笑脸，同时该游客快乐度回到初始快乐度
(7)	鼠标左键键点击可采集笑脸，从而将其转化为一定量的欢笑精华。笑脸有存在时间限制，超时则消失
(8)	若建筑间的判定范围圈出现重叠，则当游客进入重叠区时，游客会对其更加偏好的那个建筑进行是否要进入的判定，若游客对重叠区的两个或以上建筑的偏好相同，则随机对其中的一个建筑进行判定
(9)	游客有三种情绪状态：一般、生气与开心，处于生气状态的游客会“暴走”即加速移动直至情绪状态变为一般或开心
(10)	情绪状态为生气的游客在离开旅游区时增加场景不满度（+1）。当不满度达到10，且旅游区中仍有未离开的游客时，则视为游戏失败
(11)	若所有游客均离开旅游区时，不满度仍小于10，则视为关卡通过成功，并根据玩家在本关卡获得的总欢笑精华量及不满度评级（1星，2星，3星)
### 游戏玩法:
在景区地图中，玩家通过在建造位上建设有特点的建筑吸引游客，通过游客喜好倾向和建筑特点的匹配，尽可能多地产出资源，并尽可能让游客开心，并产出关键资源，最终达到通关要求。游戏的博弈点在于建造位置的选择、调整建筑总类以符合特定游客的喜好。
## 故事背景
YOTTA的故事稍稍带有奇幻的色彩：平行时空中的建造大师Yote希望能给人们带来更多欢乐，因此来到了某个时空中的中国，在这里的不同城市建造有趣的景区，吸引游客们，并让他们产出欢笑精华，以丰富时空中的欢乐。而玩家就要扮演建造者，在景区中建设并取悦游客们，这个故事背景在开头动画中有了详细介绍。
## 游戏配置
当前版本的游戏可以运行于大部分的台式机或者笔记本上，推荐配置：
硬件环境：
CPU： i5-3230M(双核)
内存: DDR3 1600 (4G)
显卡： NVIDIA GeForce GT 740M(独立2GB)
芯片组： 因特尔HM77芯片组
软件环境：
操作系统：Windows 7
开发软件： Unity Version 4.6.1
### 游戏操作说明
操作方式：鼠标左键点击。
具体操作：根据指示选择进入关卡，每一关卡开始，鼠标点击屏幕上的建造标记（）建造建筑物，点击已经建造好的建筑物可以升级或者出售，点击笑脸搜集欢笑精华。
## 制作人员介绍
	制作人：韩红雷

	指导老师：费广正、成星

	策划：黄邦宁、吴婉乔

	程序：徐婵婵

	美术：李萌、邹悦、张思雨

	单位：中国传媒大学艺术学部

	email：hanhonglei@cuc.edu.cn

----

- This project is under [GNU GENERAL PUBLIC LICENSE](https://www.gnu.org/licenses/), please check it in the root folder.
