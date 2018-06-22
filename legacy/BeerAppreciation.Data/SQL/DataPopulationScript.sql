-- Beer Appreciation Data Population Script
-- Matt Granger 06/2014

SET IDENTITY_INSERT [BA].[Manufacturers] ON 

INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (1, N'Yeastie Boys', NULL, N'New Zealand')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (2, N'Iron Bark Brewery', NULL, N'Australia')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (3, N'La Trappe', NULL, N'Normandy')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (4, N'Chimay', NULL, N'Belgium')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (5, N'Boneyard Brewing', NULL, N'Australia')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (6, N'Rogue', NULL, N'America')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (7, N'Matilda Bay Brewing Company', NULL, N'Australia')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (8, N'Staatliches Hofbräuhaus München', NULL, N'Germany')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (9, N'Anchor Brewing', NULL, N'America')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (10, N'Weltenburger Kloster', NULL, N'Germany')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (11, N'Sapporo', NULL, N'Japan')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (12, N'Phoenix', NULL, N'Mauritius')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (13, N'Budějovický Budvar', NULL, N'Czech')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (14, N'Batemans Brewery', NULL, N'England')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (15, N'Mountain Goat Beer', NULL, N'Australia')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (16, N'Weihenstephan', NULL, N'Bavaria')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (17, N'Coopers Brewery', NULL, N'Australia')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (18, N'Brasserie Du Bocq', NULL, N'Belgium')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (19, N'Asahi Breweries Ltd', NULL, N'Japan')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (20, N'Blue Moon Brewing Co.', NULL, N'Australia')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (21, N'Duvel', NULL, N'Belgium')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (22, N'Amager Bryghus', NULL, N'Denmark')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (23, N'Wells Wells', NULL, N'Australia')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (24, N'Murray''s Craft Brewing Company', NULL, N'Australia')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (25, N'Brad''s Brewing Company', NULL, N'Australia')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (26, N'Guinness', NULL, N'Ireland')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (27, N'Burleigh Brewing Co', NULL, N'Australia')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (28, N'Heineken', NULL, N'Netherlands')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (29, N'Sierra Nevada', NULL, N'America')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (30, N'Brasseries Kronenbourg', NULL, N'France')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (31, N'Feral', N'The Feral Brewing Company is a proudly family owned and operated hand-crafted microbrewery situated in the heart of Perth’s premier food and wine tourism precinct – the Swan Valley, just 20km from the Perth CBD. We handcraft a world class range of awarded beers expanding from the usual to the deliciously unusual.', N'Australia')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (32, N'Family Brewery', NULL, N'Australia')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (33, N'Abbey of Leffe', NULL, N'Belgium')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (34, N'Garage Project', NULL, N'New Zealand')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (35, N'South East Brewing Co', NULL, N'Australia')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (36, N'Matsos Brewery', NULL, N'Australia')
INSERT [BA].[Manufacturers] ([Id], [Name], [Description], [Country]) VALUES (37, N'Six Point', NULL, N'America')

SET IDENTITY_INSERT [BA].[Manufacturers] OFF

SET IDENTITY_INSERT [BA].[BeverageTypes] ON 

INSERT [BA].[BeverageTypes] ([Id], [Name], [Description]) VALUES (1, N'Beer', N'Delicious Beer')

SET IDENTITY_INSERT [BA].[BeverageTypes] OFF

SET IDENTITY_INSERT [BA].[BeverageStyles] ON 

INSERT [BA].[BeverageStyles] ([Id], [Name], [Description], [BeverageTypeId], [ParentId]) VALUES (1, N'Golden Ale', NULL, 1, NULL)
INSERT [BA].[BeverageStyles] ([Id], [Name], [Description], [BeverageTypeId], [ParentId]) VALUES (2, N'Dark Ale', NULL, 1, NULL)
INSERT [BA].[BeverageStyles] ([Id], [Name], [Description], [BeverageTypeId], [ParentId]) VALUES (3, N'Ale', NULL, 1, NULL)
INSERT [BA].[BeverageStyles] ([Id], [Name], [Description], [BeverageTypeId], [ParentId]) VALUES (4, N'Pale Ale', NULL, 1, NULL)
INSERT [BA].[BeverageStyles] ([Id], [Name], [Description], [BeverageTypeId], [ParentId]) VALUES (5, N'Pilsner', NULL, 1, NULL)
INSERT [BA].[BeverageStyles] ([Id], [Name], [Description], [BeverageTypeId], [ParentId]) VALUES (6, N'Amber Ale', NULL, 1, NULL)
INSERT [BA].[BeverageStyles] ([Id], [Name], [Description], [BeverageTypeId], [ParentId]) VALUES (7, N'Lager', NULL, 1, NULL)
INSERT [BA].[BeverageStyles] ([Id], [Name], [Description], [BeverageTypeId], [ParentId]) VALUES (8, N'Golden Lager', NULL, 1, NULL)
INSERT [BA].[BeverageStyles] ([Id], [Name], [Description], [BeverageTypeId], [ParentId]) VALUES (9, N'Black Lager', NULL, 1, NULL)
INSERT [BA].[BeverageStyles] ([Id], [Name], [Description], [BeverageTypeId], [ParentId]) VALUES (10, N'Brown Ale', NULL, 1, NULL)
INSERT [BA].[BeverageStyles] ([Id], [Name], [Description], [BeverageTypeId], [ParentId]) VALUES (11, N'Bavarian', NULL, 1, NULL)
INSERT [BA].[BeverageStyles] ([Id], [Name], [Description], [BeverageTypeId], [ParentId]) VALUES (12, N'German Maibock', NULL, 1, NULL)
INSERT [BA].[BeverageStyles] ([Id], [Name], [Description], [BeverageTypeId], [ParentId]) VALUES (13, N'Malt', NULL, 1, NULL)
INSERT [BA].[BeverageStyles] ([Id], [Name], [Description], [BeverageTypeId], [ParentId]) VALUES (14, N'Weizen Bock', NULL, 1, NULL)
INSERT [BA].[BeverageStyles] ([Id], [Name], [Description], [BeverageTypeId], [ParentId]) VALUES (15, N'IPA', NULL, 1, NULL)
INSERT [BA].[BeverageStyles] ([Id], [Name], [Description], [BeverageTypeId], [ParentId]) VALUES (16, N'Tripel', NULL, 1, NULL)
INSERT [BA].[BeverageStyles] ([Id], [Name], [Description], [BeverageTypeId], [ParentId]) VALUES (17, N'Munich Dunkel Lager ', NULL, 1, NULL)
INSERT [BA].[BeverageStyles] ([Id], [Name], [Description], [BeverageTypeId], [ParentId]) VALUES (18, N'Belgian White (Witbier)', NULL, 1, NULL)
INSERT [BA].[BeverageStyles] ([Id], [Name], [Description], [BeverageTypeId], [ParentId]) VALUES (19, N'Stout', NULL, 1, NULL)
INSERT [BA].[BeverageStyles] ([Id], [Name], [Description], [BeverageTypeId], [ParentId]) VALUES (20, N'Schwarzbier', NULL, 1, NULL)
INSERT [BA].[BeverageStyles] ([Id], [Name], [Description], [BeverageTypeId], [ParentId]) VALUES (21, N'Wheat Beer', NULL, 1, NULL)

SET IDENTITY_INSERT [BA].[BeverageStyles] OFF

SET IDENTITY_INSERT [BA].[DrinkingClubs] ON 

INSERT INTO [BA].[DrinkingClubs] ([Id], [Name] ,[Description] ,[PasswordHash], [IsPrivate]) VALUES(1, 'Research & Development', 'Inaugral Drinking Club', null, 0);

SET IDENTITY_INSERT [BA].[DrinkingClubs] OFF


SET IDENTITY_INSERT [BA].[Events] ON 

INSERT [BA].[Events] ([Id], [Date], [Name], [DrinkingClubId]) VALUES (1, CAST(0x0000A30200000000 AS DateTime), N'Beer Appreciation Evening #1', 1)
INSERT [BA].[Events] ([Id], [Date], [Name], [DrinkingClubId]) VALUES (2, CAST(0x0000A31E00000000 AS DateTime), N'Beer Appreciation Evening #2', 1)
INSERT [BA].[Events] ([Id], [Date], [Name], [DrinkingClubId]) VALUES (3, CAST(0x0000A34800000000 AS DateTime), N'Beer Appreciation Evening #3', 1)
INSERT [BA].[Events] ([Id], [Date], [Name], [DrinkingClubId]) VALUES (4, CAST(0x0000A36A00000000 AS DateTime), N'Beer Appreciation Evening #4', 1)

SET IDENTITY_INSERT [BA].[Events] OFF

SET IDENTITY_INSERT [BA].[Beverages] ON 

INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (2, N'La Trappe Blonde', N'This scintillating golden ale boasts a rich, fruity, and fresh aroma. And a light malty and sweet taste. It has a soft bitterness with a friendly aftertaste. A well-balanced blend of complexity and simplicity. La Trappe Blond continues to ferment in the bottle.', CAST(6.50 AS Decimal(18, 2)), 330, 3, 1, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (3, N'Chimay Blue', N'This authentic Belgian beer, whose tinge of fresh yeast is associated with a light rosy flowery touch, is particularly pleasant. Its aroma, perceived as one enjoys it, only accents the delightful sensations revealed by the odour, all revealing a light but agreeable caramelized note. ', CAST(9.00 AS Decimal(18, 2)), 330, 4, 1, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (4, N'Brumby Ale', N'Mexican style beer', CAST(4.40 AS Decimal(18, 2)), 330, 2, 3, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (10, N'Blokes Brown', N'Full-flavoured malty dark beer.', CAST(0.00 AS Decimal(18, 2)), 330, 2, 2, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (13, N'Oz Brewing Pale Ale', NULL, CAST(0.00 AS Decimal(18, 2)), 330, 2, 4, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (15, N'Aussie Pils', N'Crisp beer using Czech pilsner yeast and Saaz hops.', CAST(4.40 AS Decimal(18, 2)), 330, 2, 5, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (17, N'Rex Attitude', N'The world''s first heavily-peated single malt ale. A deceptively innocuous game changing beer that is possibly the smokiest in the world, certainly one of the most polarising, and yet very subtle and beguilingly drinkable for those who get past the initial shock.', CAST(7.00 AS Decimal(18, 2)), 330, 1, 1, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (20, N'Boneyard Golden', N'Aggressively hopped and inherently unbalanced, Boneyard Golden Ale is a session beer for hop junkies. A combination of Australian, New Zealand and American hops deliver a complex aroma (think mango, peach, lychee, passionfruit and guava) and firm, resinous bitterness which lingers into the dry, moreish finish.', CAST(4.50 AS Decimal(18, 2)), 330, 5, 1, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (21, N'Debilitator', N'Full mouthed, rich, dark Belgium style beer.', CAST(7.00 AS Decimal(18, 2)), 330, 2, 2, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (22, N'Rogue Farms Chipotle Ale', N'Deep amber in color with a rich malty aroma, delicately spiced with smoked Jalapeno peppers to give it that extra bite!', CAST(0.00 AS Decimal(18, 2)), 650, 6, 6, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (23, N'Minimum Chips', N'A classic golden lager that uses pale and crystal malts to produce a mild malty palate.', CAST(4.70 AS Decimal(18, 2)), 345, 7, 8, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (24, N'Hazelnut Brown Nectar', N'A nutty twist to a traditional European Brown Ale. Dark brown in color with a hazelnut aroma, a rich nutty flavor and a smooth malty finish.', CAST(0.00 AS Decimal(18, 2)), 650, 6, 10, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (25, N'Hofbräu München Dunkel', N'Dark beer existed in Bavaria long before light beer. This was the first type of beer to be brewed at Hofbräuhaus when it was founded. Today, when beer-lovers all over the world talk about dark beer, they usually mean a Munich style beer. Today, Hofbräu Dunkel - the archetypal Bavarian beer - is still as popular as ever. With its alcoholic content of around 5.5% volume and its spicy taste, it’s a refreshing beer that suits all kinds of occasion.', CAST(5.50 AS Decimal(18, 2)), 345, 8, 11, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (27, N'Anchor Steam Beer', N'Anchor Steam® Beer owes its deep amber color, thick, creamy head, and rich, distinctive flavor to a historic brewing process like none other.

It is a process that combines deep respect for craft brewing tradition with many decades of evolution to arrive at a unique approach: a blend of pale and caramel malts, fermentation with lager yeast at warmer ale temperatures in shallow open-air fermenters, and gentle carbonation in our cellars through an all-natural process called kräusening.

', CAST(4.90 AS Decimal(18, 2)), 345, 9, 6, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (28, N'Barock Dunkel', N'The world´s oldest dark beer. 
A true experience of flavour. 

Full bodied, with fine malt aromas. slightly bitter with some sweetness, creamy and with the warm colours of amber, intensive flavour and fragrance.

Brewed in traditional monastic manner, its one of the great beers.', CAST(0.00 AS Decimal(18, 2)), 345, 10, 6, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (29, N'Dead Guy Ale', N'Deep reddish amber hue. Generous toasty malt aromas and earthy hops follow through on a moderately full-bodied palate with fruity accents and a long spicy hop finish. A delicious hybrid style with bock-like maltiness but ale-like hopping.', CAST(0.00 AS Decimal(18, 2)), 650, 6, 12, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (30, N'Hannans', N'A clean typical Australian Style lager. Initial sweetness then finishes with a slight bitterness.', CAST(5.50 AS Decimal(18, 2)), 345, 2, 7, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (31, N'Sapporo', N'Sapporo Premium Beer is a perfectly balanced golden lager brewed with the Japanese attention to detail. The slow cool fermentation balances delicate hops and esters with a full malt character. The authentic brewing techniques and quality ingredients used have produced a crisp and refreshing lager beer.', CAST(0.00 AS Decimal(18, 2)), 650, 11, 7, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (32, N'Phoenix Beer', N'A polished, golden yellow beer with 5% alcohol. Phoenix beer is pasteurised after bottling, according to natural conservation methods.
The quality of the underground water also allows us to produce a beer with no chemical additives.', CAST(5.00 AS Decimal(18, 2)), 345, 12, 7, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (33, N'Budweiser Budvar', N'The name of the brewery Budějovický Budvar and the beer Budweiser Budvar relates to the place of its origin – the town of České Budějovice. Since the 14th century the official name of this city was Budweis. Only in 1918 was the name changed into the Czech name of České Budějovice. However, the indication Budweis is today the official translation of the name of the city into many foreign languages.', CAST(0.00 AS Decimal(18, 2)), 345, 13, 7, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (34, N'Yebisu', N'A Dortmunder style lager brewed from 100% by Sapporo. First brewed in Tokyo in 1890, the city''s neighbourhood of Ebisu is named after this beer which was originally produced there. (The Y is silent when pronounced.) The brand was forgotten until resurrected in 1971 as Sapporo''s ''premium'' beer.', CAST(5.00 AS Decimal(18, 2)), 750, 11, 13, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (35, N'Dark Lord', N'Dark Lord is a hoppy bitter-sweet dark ale with a finish of hazelnuts. It is an ideal accompaniment to roast beef or venison. Dark, Delicious & Delectable.', CAST(4.70 AS Decimal(18, 2)), 500, 14, 2, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (36, N'Fancy Pants', N'Think Hightail Ale but souped up with more malt and plenty of Galaxy hops', CAST(5.20 AS Decimal(18, 2)), 375, 15, 7, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (37, N'Weihenstephaner Vitus', N'A light-coloured, spicy single-bock wheat beer, for both beer lovers and the beer connoisseur. Extra long and cold storage in our monastery cellars makes this single-bock a really special beer with full body and a distinctively great mouthfeel.', CAST(7.70 AS Decimal(18, 2)), 375, 16, 14, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (38, N'Coopers Sparkling Ale', N'The pinnacle of the brewers’ craft. The ale by which all others should be measured. With its famous cloudy sediment and its distinctive balance of malt, hops and fruity characters, the old ‘Red Label’ is a tasty slice of Coopers history.', CAST(5.80 AS Decimal(18, 2)), 375, 17, 3, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (39, N'Anchor Liberty Ale', N'The champagne-like bubbles, distinctive hop bouquet, and balanced character of Liberty Ale® revives centuries-old ale brewing traditions that are now more relevant than ever.	First introduced in 1975, Liberty Ale® is brewed strictly according to traditional craft brewing methods, and uses only natural ingredients — pale malted barley, fresh whole-cone Cascade hops and a special top-fermenting yeast, and water. ', CAST(5.90 AS Decimal(18, 2)), 345, 9, 15, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (40, N'Triple Moine', NULL, CAST(7.30 AS Decimal(18, 2)), 650, 18, 16, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (41, N'Asahi Black', N'A premium dark ale', CAST(5.00 AS Decimal(18, 2)), 375, 19, 17, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (42, N'Blanche de Namur', N'Blond, cloudy, milky (colloïdal by cold). The cloudy appearance is created at low temperature but disappears when the beer gets warmer or when it is kept too long in the cold. Fine fruity with a scent of the used spice, coriander and bitter orange. Smooth beer, thirst-quenching but mild, slightly acidulous, powdery consistency on the tongue, not bitter', CAST(4.50 AS Decimal(18, 2)), 345, 18, 18, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (51, N'Blue Moon', N'It started with our brewmaster, Keith Villa, wanting to craft a beer inspired by the flavorful Belgian Wits he enjoyed while studying brewing in Belgium. He brewed his interpretation using Valencia orange peel versus the traditional tart Curaçao orange peel, for a subtle sweetness. Then he added oats and wheat to create a smooth, creamy finish that’s inviting to the palate. As a final touch, he garnished the beer with an orange slice to heighten the citrus aroma and taste.', CAST(0.00 AS Decimal(18, 2)), 345, 20, 18, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (52, N'Duvel Golden Ale', N'Blonde and refreshing but with flavour and complexity, the Belgian Ale defines the strong Golden Ale. Made with Pilsner malts, Bohemian hops and a unique yeast strain, creating an intense, aromatic and beguiling Ale.', CAST(8.50 AS Decimal(18, 2)), 330, 21, 4, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (53, N'Gnädige Frau Marshmallow Stout', N' “Gnädige Frau - Marshmallow Stout” - combining a trademark Danish beer with the very American campfire treat a s’more. The name of the beer is a tribute to Kristina Bozic, the current owner of West Lakeview Liquors who personally and enthusiastically took part in the brewing here at Amager Bryghus, slicing vanilla beans and marshmallows and crushing graham crackers with a meat hammer! A sweet and unique lady requires a very special beer.', CAST(10.00 AS Decimal(18, 2)), 750, 22, 19, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (54, N'Coopers Best Extra Stout', N'Coopers Best Extra Stout is as close to perfection any stout is likely to ever get', CAST(6.30 AS Decimal(18, 2)), 375, 17, 19, 1, N'http://coopers.com.au/#/our-beer/ales-stout/best-extra-stout/')
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (55, N'Banana Bread Beer', N'A truly unique beer experience - this is a classic English ale with a twist. Fair-trade Bananas are blended with the finest malts to give a rich Banoffee flavour in this rich, silky and entirely delicious beer.', CAST(0.00 AS Decimal(18, 2)), 500, 23, 3, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (56, N'Angry Man Pale Ale', N'Murray''s Angry Man Pale Ale is a lively ale - brilliant light golden in colour, with a full bodied finish and complex character. Well balanced with biscuit/toffee flavours.', CAST(0.00 AS Decimal(18, 2)), 375, 24, 4, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (57, N'Black Drip Lager', N'Brad and Ory home brew', NULL, 375, 25, 7, 1, N'http://bradsucks.net')
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (58, N'Guinness Black Lager', N'Guinness Black Lager is cold-brewed with roasted barley to deliver the refreshing taste of lager with the unique character of Guinness. Enjoy ice cold straight from the bottle.', CAST(4.50 AS Decimal(18, 2)), 375, 26, 20, 1, N'http://www.guinness.com/en-au/thelager.html')
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (59, N'Hassle Hop', NULL, NULL, 650, 27, 4, 1, N'http://http://www.burleighbrewing.com.au/')
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (60, N'Newcastle Brown Ale', N'Launched in 1927 by Colonel Jim Porter after three years of development, the merger of Newcastle Breweries with Scottish Brewers afforded the beer national distribution and United Kingdom sales peaked in the early 1970s', CAST(4.70 AS Decimal(18, 2)), 500, 28, 10, 1, NULL)
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (61, N'Torpedo Extra IPA', N'Sierra Nevada and hops go hand in hand. What began as a crazy idea scribbled in a pub eventually became our newest year-round hop bomb, Torpedo Extra IPA. The first beer to feature our “Hop Torpedo”—a revolutionary dry-hopping device that controls how much hop aroma is imparted into beer without adding additional bitterness. Torpedo Extra IPA is an aggressive yet balanced beer with massive hop aromas of citrus, pine, and tropical fruit.', CAST(7.20 AS Decimal(18, 2)), 375, 29, 15, 1, N'http://www.sierranevada.com/beer/year-round/torpedo-extra-ipa')
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (62, N'Weltenburger Kloster Pils', N'Fresh and pleasantly bitter, hoppy in scent and flavour, golden with a creamy, compact head.
It satisfies the highest requirements and plays in the Premium League of Pils beers.', CAST(4.90 AS Decimal(18, 2)), 375, 10, 5, 1, N'http://www.weltenburger.de/en/beer-treat/weltenburg-beers/pils/')
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (63, N'Kronenbourg 1664 Blanc', N'1664 Blanc is an original wheat beer. It is a different, fresh and fruity white beer, slightly bitter with hints of citrus and coriander spices. With its modern and elegant blue bottle, 1664 Blanc is perfectly adapted to all moments of conviviality.', CAST(5.00 AS Decimal(18, 2)), 330, 30, 18, 1, N'http://www.carlsberggroup.com/brands/Pages/KronenbourgBlanc1664.aspx')
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (64, N'Feral White', N'Declared Best Lager in the World in this year''s World Beer Awards, run by Beers of the World magazine, Budweiser Budvar Dark is a retro-beer. This is because it has been designed to come as close as possible to how all Bohemian and Bavarian lagers tasted before bottom fermented golden lager stole the show in the mid nineteenth century. Now, thanks to Budweiser Budvar, the Darkside is moving centre stage again. Enjoying the same brewing cycle as Original, Dark gets its delicious roasted flavour from being brewed from three types of malt; Munich, caramel and roasted.', CAST(4.6 AS Decimal(18, 2)), 375, 31, 18, 1, N'https://www.feralbrewing.com.au/feral-beer/')
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (65, N'Whale Ale', N'Murray''s Whale Ale is a refreshing wheat beer with a twist. Its high percentage of malted and unmalted wheat and aromatic late hop profile gives a unique take on a session strength ale. A classic light body, creamy mouth feel and refreshing citric flavour. This is balanced with assertive late hopping, giving a fresh, light tropical fruit aroma and cleansing dry finish. Murray''s Whale Ale is light gold in colour with the traditional cloudy appearance of wheat beers. Summer in a glass!', CAST(4.5 AS Decimal(18, 2)), 330, 24, 21, 1, N'http://www.murraysbrewingco.com.au/beers/whale-ale/')
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (66, N'Delirium Tremens', N'', CAST(8.5 AS Decimal(18, 2)), 330, 32, 18, 1, N'')
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (67, N'Leffe', N'', CAST(8.5 AS Decimal(18, 2)), 330, 33, 18, 1, N'')
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (68, N'Angry Peaches', N'', CAST(8.5 AS Decimal(18, 2)), 330, 34, 18, 1, N'')
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (69, N'Kaiju Metamorphasis', N'', CAST(8.5 AS Decimal(18, 2)), 330, 35, 18, 1, N'')
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (70, N'Chimay Red', N'', CAST(8.5 AS Decimal(18, 2)), 330, 4, 18, 1, N'')
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (71, N'Mango Beer', N'', CAST(8.5 AS Decimal(18, 2)), 330, 36, 18, 1, N'')
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (72, N'Budvar Dark', N'', CAST(8.5 AS Decimal(18, 2)), 330, 13, 18, 1, N'')
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (73, N'Resin', N'', CAST(8.5 AS Decimal(18, 2)), 330, 37, 18, 1, N'')
INSERT [BA].[Beverages] ([Id], [Name], [Description], [AlcoholPercent], [Volume], [ManufacturerId], [BeverageStyleId], [BeverageTypeId], [Url]) VALUES (74, N'Sly Fox', N'', CAST(8.5 AS Decimal(18, 2)), 330, 31, 18, 1, N'')

SET IDENTITY_INSERT [BA].[Beverages] OFF

