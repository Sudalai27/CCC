(function () {
    'use strict';
    var app = angular.module('ProsolApp', ['cgBusy', 'angular.filter','datatables']);

    app.controller('ToolsController', function ($scope, $http, $rootScope, $timeout, $filter) {

        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        //$scope.label = $('#lbl').val();
        $scope.label = "";
        $scope.BtnFARmodel = 'Select FAR ID';
        $scope.BtnRegmodel = 'Select Region';
        $scope.BtnDescmodel = '';
        $scope.maxInitialRecords = 10;
        $scope.obj = {};

        $scope.pageSize = 10; 
        $scope.currentPage = 1;

        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/FAR/GetDataList',
                params: { label: $scope.label }
            }).success(function (response) {
                $scope.masterList = response;
                console.log($scope.masterList)
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };

        $scope.BindAllList = function () {

            $http({
                method: 'GET',
                url: '/FAR/GetAllDataList',
            }).success(function (response) {
                $scope.masterAllList = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindAllList();

        $scope.onclickMfr = function () {
            $scope.obj = {};
            $scope.label = "Manufacturer";
            console.log($scope.label)
            $scope.BindList();
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
        }
        $scope.onclickMfr();

        $scope.onclickLbl = function (lbl) {
            $scope.obj = {};
            $scope.label = lbl;
            console.log($scope.label)
            $scope.BindList();
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
        }

        // Function to load the next page
        $scope.loadNextPage = function () {
            if ($scope.currentPage < $scope.totalPages) {
                $scope.currentPage++;
                $scope.BindList();
            }
        };

        // Function to load the previous page
        $scope.loadPreviousPage = function () {
            if ($scope.currentPage > 1) {
                $scope.currentPage--;
                $scope.BindList();
            }
        };

        // Initial load


        $scope.regionFilter = function (far) {
            return far.Region !== null && far.Region !== "";
        };
        $scope.assetDescFilter = function (far) {
            return far.Region !== null && far.AssetDesc !== null;
        };

        $scope.changeFar = function (far) {

            if (far != null) {
                $scope.BtnFARmodel = far;
                $scope.obj.FARId = far;
                //if()
            }
            $scope.RegionMaster_ = $filter('filter')($scope.FARMaster, function (i) {
                return i.FARId == far;
            });
            $scope.RegionList = Array.from(new Set($scope.RegionMaster_.map(i => i.Region)));
            //console.log($scope.RegionList)
        }
        $scope.changeReg = function (reg) {

            if (reg != null) {
                $scope.BtnRegmodel = reg;
                $scope.obj.Region = reg;
                //if()
            }

        }
        $scope.onclickFL = function () {
            $scope.obj = {};
            $scope.BindFL();
            $scope.resetFL();
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
        }

        $scope.resetFL = function () {
            $scope.fl = {
                Level1: "",
                Level2: "",
                Level3: "",
                Level4: "",
                Level5: "",
                Level6: "",
                Level7: "",
                Equipment: "",
                PrimaryEquipment: "",
                SubEquipment1: "",
                SubEquipment2: "",
                SubEquipment3: "",
                SuperiorLocation: "",
                SectionNo: "",
                Sequence: "",
                UniqueId: "",
                FunctLocation: "",
            };
        }

        $scope.fl = {
            Level1 : "",
            Level2 : "",
            Level3 : "",
            Level4 : "",
            Level5 : "",
            Level6 : "",
            Level7 : "",
            Equipment : "",
            PrimaryEquipment : "",
            SubEquipment1 : "",
            SubEquipment2 : "",
            SubEquipment3 : "",
            SuperiorLocation : "",
            SectionNo : "",
            Sequence : "",
            UniqueId : "",
            FunctLocation : "",
        };

        var supLoc = "";
        var funcLoc = "";
        $scope.nFLArray= [];
        $scope.BindFL = function () {
            $http({
                method: 'GET',
                url: '/FAR/GetFuncLoc',
            }).success(function (response) {
                $scope.flList = response;
                console.log($scope.flList)
            }).error(function (err){
                console.log(err);
            })
        }

        $scope.splitCode = function (code) {
            const parts = code.split('-');
            let result = [];
            for (let i = 0; i < parts.length; i++) {
                result.push(parts.slice(0, i + 1).join('-'));
            }
            return result;
        };

        //function buildFLTree(data) {
        //    let root = [];
        //    let map = {};

        //    data.forEach(fullCode => {
        //        // Split full code by "-" and progressively build up the path
        //        const parts = fullCode.split('-');
        //        let currentLevel = root;
        //        let currentPath = '';

        //        for (let i = 0; i < parts.length; i++) {
        //            // Build cumulative path like CCP, CCP-1200, CCP-1200-LSCC, ...
        //            currentPath = currentPath ? currentPath + '-' + parts[i] : parts[i];

        //            // Check if this node already exists in map
        //            let existing = map[currentPath];
        //            if (!existing) {
        //                existing = {
        //                    name: currentPath,
        //                    children: [],
        //                    expanded: false
        //                };
        //                map[currentPath] = existing;
        //                currentLevel.push(existing);
        //            }

        //            // Go deeper for next iteration
        //            currentLevel = existing.children;
        //        }
        //    });

        //    return root;
        //}


        $scope.isClicked = false;
        $scope.flTreeData = [];
        $scope.flData = [];
        $scope.getbOM = function (id, fl) {
            $scope.isClicked = false;

            // 1️⃣ Get BOM data first
            $http({
                method: 'GET',
                url: '/FAR/getFLBom',
                params: { id: fl }
            }).success(function (bomResponse) {
                console.log("BOM Data:", bomResponse);

                // 2️⃣ Then get Functional Location data
                $http({
                    method: 'GET',
                    url: '/FAR/getNestedFL',
                    params: { id: fl }
                }).success(function (flResponse) {
                    console.log("FL Data:", flResponse);

                    // Assign raw data for debugging or reuse
                    $scope.flData = flResponse;
                    $scope.bomData = bomResponse;

                    // Build FL tree
                    $scope.treeFLData = buildFLTree($scope.flData);
                    console.log("FL Tree:", $scope.treeFLData);

                    // Merge FL and BOM trees
                    $scope.treeBOMData = buildMergedTree($scope.flData, $scope.bomData);
                    console.log("Merged Tree:", $scope.treeBOMData);
                    $scope.isClicked = true;
                }).error(function (data, status) {
                    console.error("Error fetching FL data:", status);
                });
            }).error(function (data, status) {
                console.error("Error fetching BOM data:", status);
            });
        };




        //old
        //function buildFLTree(equipmentData) {
        //    const root = [];
        //    const map = {};

        //    // Build base structure by splitting FunctLocation
        //    equipmentData.forEach(item => {
        //        const fullCode = item.FunctLocation;
        //        const parts = fullCode.split('-');
        //        let currentLevel = root;
        //        let currentPath = '';

        //        for (let i = 0; i < parts.length; i++) {
        //            currentPath = currentPath ? currentPath + '-' + parts[i] : parts[i];
        //            let existing = map[currentPath];
        //            if (!existing) {
        //                existing = {
        //                    name: currentPath,
        //                    fullCode: currentPath,
        //                    children: [],
        //                    expanded: false,
        //                    type: 'node'
        //                };
        //                map[currentPath] = existing;
        //                currentLevel.push(existing);
        //            }
        //            currentLevel = existing.children;
        //        }
        //    });

        //    // Enrich nodes with equipment labels and classify type
        //    equipmentData.forEach(item => {
        //        const node = map[item.FunctLocation];
        //        if (node) {
        //            if (item.PrimaryEquipment) {
        //                node.name += ` (${item.PrimaryEquipment} - Primary Equipment)`;
        //                node.type = "primary";
        //            }
        //            else if (item.SubEquipment1) {
        //                node.name += ` (${item.SubEquipment1} - Sub Equipment 1)`;
        //                node.type = "sub1";
        //            }
        //            else if (item.SubEquipment2) {
        //                node.name += ` (${item.SubEquipment2} - Sub Equipment 2)`;
        //                node.type = "sub2";
        //            }
        //            else if (item.SubEquipment3) {
        //                node.name += ` (${item.SubEquipment3} - Sub Equipment 3)`;
        //                node.type = "sub3";
        //            }
        //        }
        //    });

        //    // Now nest sub2 under their nearest sub1 parent if prefix matches
        //    equipmentData.forEach(item => {
        //        if (item.SubEquipment2) {
        //            const sub2Node = map[item.FunctLocation];
        //            const prefix = item.FunctLocation.split('.')[0]; // e.g. CCP-1200-...-212BC1
        //            const possibleSub1Parent = Object.values(map).find(n =>
        //                n.fullCode.startsWith(prefix) && n.type === "sub1"
        //            );
        //            if (sub2Node && possibleSub1Parent && !possibleSub1Parent.children.includes(sub2Node)) {
        //                // Remove from previous position
        //                Object.values(map).forEach(p => {
        //                    const idx = p.children.indexOf(sub2Node);
        //                    if (idx !== -1) p.children.splice(idx, 1);
        //                });
        //                // Add under sub1 parent
        //                possibleSub1Parent.children.push(sub2Node);
        //            }
        //        }
        //    });

        //    return root;
        //}

        function buildFLTree(equipmentData) {
            const root = [];
            const map = {};

            // Build base structure by splitting FunctLocation
            equipmentData.forEach(item => {
                const fullCode = item.FunctLocation;
                const parts = fullCode.split('-');
                let currentLevel = root;
                let currentPath = '';

                for (let i = 0; i < parts.length; i++) {
                    currentPath = currentPath ? currentPath + '-' + parts[i] : parts[i];
                    let existing = map[currentPath];
                    if (!existing) {
                        existing = {
                            name: currentPath,
                            fullCode: currentPath,
                            children: [],
                            expanded: false,
                            type: 'node'
                        };
                        map[currentPath] = existing;
                        currentLevel.push(existing);
                    }
                    currentLevel = existing.children;
                }
            });

            // Enrich nodes with equipment labels and classify type
            equipmentData.forEach(item => {
                const node = map[item.FunctLocation];
                if (node) {
                    if (item.PrimaryEquipment) {
                        node.name = ` (Primary Equipment) ` + node.name ;
                        node.type = "primary";
                    }
                    else if (item.SubEquipment1) {
                        node.name = ` (Sub Equipment) ` + node.name ;
                        node.type = "sub1";
                    }
                    else if (item.SubEquipment2) {
                        node.name = ` (Sub-Sub Equipment) ` + node.name ;
                        node.type = "sub2";
                    }
                    else if (item.SubEquipment3) {
                        node.name = ` (Sub-Sub-Sub Equipment 3) ` + node.name ;
                        node.type = "sub3";
                    }
                }
            });

            // ✅ Nest SubEquipment1 under its Primary Equipment
            equipmentData.forEach(item => {
                if (item.SubEquipment1) {
                    const subNode = map[item.FunctLocation];
                    // Find parent Primary Equipment based on prefix
                    const prefix = item.FunctLocation.split('-').slice(0, -1).join('-');
                    const possibleParent = Object.values(map).find(n =>
                        n.fullCode.startsWith(prefix) && n.type === "primary"
                    );

                    if (subNode && possibleParent && !possibleParent.children.includes(subNode)) {
                        // Remove from previous parent
                        Object.values(map).forEach(p => {
                            const idx = p.children.indexOf(subNode);
                            if (idx !== -1) p.children.splice(idx, 1);
                        });
                        possibleParent.children.push(subNode);
                    }
                }
            });

            // ✅ Nest SubEquipment2 under its nearest SubEquipment1
            equipmentData.forEach(item => {
                if (item.SubEquipment2) {
                    const sub2Node = map[item.FunctLocation];
                    const prefix = item.FunctLocation.split('-').slice(0, -1).join('-');
                    const possibleParent = Object.values(map).find(n =>
                        n.fullCode.startsWith(prefix) && n.type === "sub1"
                    );
                    if (sub2Node && possibleParent && !possibleParent.children.includes(sub2Node)) {
                        Object.values(map).forEach(p => {
                            const idx = p.children.indexOf(sub2Node);
                            if (idx !== -1) p.children.splice(idx, 1);
                        });
                        possibleParent.children.push(sub2Node);
                    }
                }
            });

            // ✅ Nest SubEquipment3 under its nearest SubEquipment2
            equipmentData.forEach(item => {
                if (item.SubEquipment3) {
                    const sub3Node = map[item.FunctLocation];
                    const prefix = item.FunctLocation.split('-').slice(0, -1).join('-');
                    const possibleParent = Object.values(map).find(n =>
                        n.fullCode.startsWith(prefix) && n.type === "sub2"
                    );
                    if (sub3Node && possibleParent && !possibleParent.children.includes(sub3Node)) {
                        Object.values(map).forEach(p => {
                            const idx = p.children.indexOf(sub3Node);
                            if (idx !== -1) p.children.splice(idx, 1);
                        });
                        possibleParent.children.push(sub3Node);
                    }
                }
            });

            return root;
        }



        //old
        //function buildTree(flatData) {
        //    // Normalize
        //    flatData.forEach(i => {
        //        i.children = i.children || [];
        //        i.expanded = false;
        //    });

        //    // Group by UniqueId
        //    const groups = flatData.reduce((acc, item) => {
        //        const uid = item.UniqueId || '__no_uid__';
        //        acc[uid] = acc[uid] || [];
        //        acc[uid].push(item);
        //        return acc;
        //    }, {});

        //    const results = [];

        //    // Helper: find longest prefix parent
        //    function findLongestPrefixParent(childId, candidateKeys) {
        //        if (!childId) return null;
        //        let best = null, bestLen = 0;
        //        for (const key of candidateKeys) {
        //            if (!key) continue;
        //            if (childId === key || childId.startsWith(key + '-') || childId.startsWith(key + '/')) {
        //                if (key.length > bestLen) { best = key; bestLen = key.length; }
        //            }
        //        }
        //        return best;
        //    }

        //    // Process each UniqueId group
        //    Object.keys(groups).forEach(uid => {
        //        const group = groups[uid];

        //        const shMap = {}, sshMap = {}, assemblyMap = {}, siMap = {};
        //        const shKeys = [], sshKeys = [], assemblyKeys = [], siKeys = [];

        //        // Create root
        //        const hItem = group.find(x => x.Category === 'H');
        //        const root = hItem ? {
        //            id: hItem.UniqueId || ('root-' + (hItem.BOMId || Math.random())),
        //            BOMId: hItem.BOMId,
        //            BOMDesc: hItem.BOMDesc,
        //            Category: 'H',
        //            UOM: hItem.UOM,
        //            Quantity: hItem.Quantity,
        //            Materialcode: hItem.ComponentId,
        //            Tag: hItem.TechIdentNo,
        //            children: [],
        //            expanded: false,
        //            _sourceItems: [hItem]
        //        } : {
        //            id: uid,
        //            BOMId: null,
        //            BOMDesc: 'Root (' + uid + ')',
        //            Category: 'H',
        //            children: [],
        //            expanded: false,
        //            _sourceItems: []
        //        };

        //        // Create SH, SSH, I, SI
        //        group.forEach(item => {
        //            const cat = item.Category;
        //            const node = {
        //                id: item.AssemblyId || (cat + '-' + Math.random()),
        //                AssemblyId: item.AssemblyId,
        //                AssemblyDesc: item.AssemblyDesc || item.BOMDesc,
        //                Category: cat,
        //                UOM: item.UOM,
        //                Quantity: item.Quantity,
        //                Tag: item.TechIdentNo,
        //                BOMId: item.BOMId,
        //                children: [],
        //                expanded: false,
        //                _sourceItem: item
        //            };
        //            if (cat === 'SH') { shMap[node.AssemblyId] = node; shKeys.push(node.AssemblyId); }
        //            else if (cat === 'SSH') { sshMap[node.AssemblyId] = node; sshKeys.push(node.AssemblyId); }
        //            else if (cat === 'I') { assemblyMap[node.AssemblyId] = node; assemblyKeys.push(node.AssemblyId); }
        //            else if (cat === 'SI') { siMap[node.AssemblyId] = node; siKeys.push(node.AssemblyId); }
        //        });

        //        // Attach SH under root
        //        Object.values(shMap).forEach(sh => root.children.push(sh));

        //        // Attach SSH under SH
        //        Object.values(sshMap).forEach(ssh => {
        //            const parentKey = findLongestPrefixParent(ssh.AssemblyId, shKeys);
        //            if (parentKey && shMap[parentKey]) shMap[parentKey].children.push(ssh);
        //            else root.children.push(ssh);
        //        });

        //        // Attach Assemblies (I)
        //        Object.values(assemblyMap).forEach(asm => {
        //            const parentKey = findLongestPrefixParent(asm.AssemblyId, shKeys);
        //            if (parentKey && shMap[parentKey]) shMap[parentKey].children.push(asm);
        //            else root.children.push(asm);
        //        });

        //        // Attach Sub-Assemblies (SI)
        //        Object.values(siMap).forEach(sa => {
        //            const parentAsmKey = findLongestPrefixParent(sa.AssemblyId, assemblyKeys);
        //            if (parentAsmKey && assemblyMap[parentAsmKey]) {
        //                assemblyMap[parentAsmKey].children.push(sa);
        //            } else {
        //                const parentSHKey = findLongestPrefixParent(sa.AssemblyId, shKeys);
        //                if (parentSHKey && shMap[parentSHKey]) shMap[parentSHKey].children.push(sa);
        //                else root.children.push(sa);
        //            }
        //        });

        //        // Attach Components (L/T/E)
        //        group.forEach(item => {
        //            if (!['L', 'T', 'E'].includes(item.Category)) return;

        //            const comp = {
        //                ComponentId: item.ComponentId,
        //                ComponentDesc: item.ComponentDesc,
        //                Materialcode: item.ComponentId,
        //                UOM: item.UOM,
        //                Quantity: item.Quantity,
        //                Category: item.Category,
        //                Tag: item.TechIdentNo,
        //                children: [],
        //                expanded: false,
        //                _sourceItem: item
        //            };

        //            const aid = item.AssemblyId;
        //            const bid = item.BOMId;
        //            let attached = false;

        //            // Attach under SI
        //            if (aid) {
        //                const bestSI = findLongestPrefixParent(aid, siKeys);
        //                if (bestSI && siMap[bestSI]) { siMap[bestSI].children.push(comp); attached = true; }
        //            }

        //            // Attach under I
        //            if (!attached && aid) {
        //                const bestI = findLongestPrefixParent(aid, assemblyKeys);
        //                if (bestI && assemblyMap[bestI]) { assemblyMap[bestI].children.push(comp); attached = true; }
        //            }

        //            // Attach under SSH (if its AssemblyId matches prefix)
        //            if (!attached && aid) {
        //                const bestSSH = findLongestPrefixParent(aid, sshKeys);
        //                if (bestSSH && sshMap[bestSSH]) { sshMap[bestSSH].children.push(comp); attached = true; }
        //            }

        //            // 🔹 Attach under SH if BOMId matches (direct component under SH)
        //            if (!attached && bid) {
        //                const directSH = Object.values(shMap).find(sh => sh.BOMId === bid);
        //                if (directSH) { directSH.children.push(comp); attached = true; }
        //            }

        //            // 🔹 Attach under root if BOMId matches (direct component under H)
        //            if (!attached && root.BOMId === bid) {
        //                root.children.push(comp);
        //                attached = true;
        //            }

        //            // Fallback
        //            if (!attached) root.children.push(comp);
        //        });

        //        // Sort for clarity
        //        const weight = n => {
        //            const order = { 'L': 1, 'T': 1, 'E': 1, 'SI': 3, 'I': 4, 'SSH': 5, 'SH': 6, 'H': 0 };
        //            return order[n.Category] || 99;
        //        };
        //        function sortRec(node) {
        //            if (!node.children || !node.children.length) return;
        //            node.children.sort((a, b) => weight(a) - weight(b));
        //            node.children.forEach(sortRec);
        //        }
        //        sortRec(root);

        //        results.push(root);
        //    });

        //    return results;
        //}

        function buildTree(flatData) {
            const rootMap = {};        // H
            const subHeaderMap = {};   // SH
            const assemblyMap = {};    // I
            const subAssemblyMap = {}; // SI

            // Initialize
            flatData.forEach(item => {
                item.children = [];
                item.expanded = false;
            });

            // STEP 1: Map BOM Headers (H)
            flatData.forEach(item => {
                if (item.Category === 'H') {
                    rootMap[item.BOMId] = {
                        BOMId: item.BOMId,
                        BOMDesc: item.BOMDesc,
                        BOMLongDesc: item.BOMLongDesc,
                        Category: 'H',
                        UOM: item.UOM,
                        Quantity: item.Quantity,
                        Materialcode: item.ComponentId,
                        Tag: item.TechIdentNo,
                        FL: item.Func_Location,
                        children: [],
                        expanded: false
                    };
                }
            });

            // STEP 2: Map SubHeaders (SH)
            flatData.forEach(item => {
                if (item.Category === 'SH') {
                    const parent = rootMap[item.BOMId];
                    if (parent) {
                        const shObj = {
                            SubHeaderId: item.AssemblyId,
                            SubHeaderDesc: item.AssemblyDesc,
                            SubHeaderLongDesc: item.AssemblyLongDesc,
                            Materialcode: item.ComponentId,
                            Category: 'SH',
                            UOM: item.UOM,
                            Quantity: item.Quantity,
                            Tag: item.TechIdentNo,
                            FL: item.Func_Location,
                            children: [],
                            expanded: false
                        };
                        parent.children.push(shObj);
                        subHeaderMap[item.AssemblyId] = shObj;
                    }
                }
            });

            // STEP 3: Map Assemblies (I)
            flatData.forEach(item => {
                if (item.Category === 'I') {
                    const assemblyObj = {
                        AssemblyId: item.AssemblyId,
                        AssemblyDesc: item.AssemblyDesc,
                        AssemblyLongDesc: item.AssemblyLongDesc,
                        Materialcode: item.ComponentId,
                        Category: 'I',
                        UOM: item.UOM,
                        Quantity: item.Quantity,
                        Tag: item.TechIdentNo,
                        FL: item.Func_Location,
                        children: [],
                        expanded: false
                    };

                    // Attach to SH if exists, else directly to its H
                    const parent =
                        subHeaderMap[item.AssemblyParentId] ||
                        rootMap[item.BOMId];

                    if (parent) parent.children.push(assemblyObj);
                    assemblyMap[item.AssemblyId] = assemblyObj;
                }
            });

            // STEP 4: Map SubAssemblies (SI)
            flatData.forEach(item => {
                if (item.Category === 'SI') {
                    const saObj = {
                        AssemblyId: item.AssemblyId,
                        AssemblyDesc: item.AssemblyDesc,
                        AssemblyLongDesc: item.AssemblyLongDesc,
                        Materialcode: item.ComponentId,
                        Category: 'SI',
                        UOM: item.UOM,
                        Quantity: item.Quantity,
                        Tag: item.TechIdentNo,
                        FL: item.Func_Location,
                        children: [],
                        expanded: false
                    };

                    const parent =
                        assemblyMap[item.AssemblyParentId] ||
                        Object.values(assemblyMap).find(a => item.AssemblyId.startsWith(a.AssemblyId + '-'));

                    if (parent) parent.children.push(saObj);
                    subAssemblyMap[item.AssemblyId] = saObj;
                }
            });

            // STEP 5: Map Components (L/T)
            flatData.forEach(item => {
                if (['L', 'T'].includes(item.Category)) {
                    const component = {
                        ComponentId: item.ComponentId,
                        ComponentDesc: item.ComponentDesc,
                        ComponentLongDesc: item.ComponentLongDesc,
                        Materialcode: item.ComponentId,
                        UOM: item.UOM,
                        Quantity: item.Quantity,
                        Category: item.Category,
                        Tag: item.TechIdentNo,
                        FL: item.Func_Location,
                        children: [],
                        expanded: false
                    };

                    let parent =
                        subAssemblyMap[item.AssemblyId] ||
                        assemblyMap[item.AssemblyId] ||
                        subHeaderMap[item.AssemblyParentId] ||
                        rootMap[item.BOMId];

                    if (parent) parent.children.push(component);
                }
            });

            // STEP 6: Sort children
            const sortChildren = (node) => {
                if (node.children && node.children.length) {
                    node.children.sort((a, b) => {
                        const order = { 'L': 1, 'T': 1, 'SI': 2, 'I': 3, 'SH': 4 };
                        return (order[a.Category] || 5) - (order[b.Category] || 5);
                    });
                    node.children.forEach(sortChildren);
                }
            };

            Object.values(rootMap).forEach(sortChildren);

            // ✅ Multiple H's become separate roots
            return Object.values(rootMap);
        }

        //old
        //function buildMergedTree(flData, flatBOMData) {
        //    // Step 1: Build the FL structure
        //    //const flTree = buildFLTree(flArray);
        //    console.log(flData)
        //    const flTree = buildFLTree(flData);
        //    console.log(flTree)
        //    // Step 2: Build the BOM tree
        //    const bomTree = buildTree(flatBOMData);
            
        //    console.log(bomTree)
        //    //// --- Download as file ---
        //    //let dataStr = JSON.stringify(bomTree, null, 2); // formatted JSON
        //    //    let blob = new Blob([dataStr], { type: "application/json" });
        //    //    let url = window.URL.createObjectURL(blob);

        //    //    // Create a temporary <a> element to trigger download
        //    //    let a = document.createElement('a');
        //    //    a.href = url;
        //    //a.download = "bomTree.json"; // file name
        //    //    document.body.appendChild(a);
        //    //    a.click();

        //    //    // Clean up
        //    //    document.body.removeChild(a);
        //    //    window.URL.revokeObjectURL(url);
        //    // Step 3: Find the last node in FL tree (deepest child)
        //    function findDeepestNode(nodes) {
        //        if (!nodes || nodes.length === 0) return null;
        //        let node = nodes[0];
        //        while (node.children && node.children.length) {
        //            node = node.children[node.children.length - 1];
        //        }
        //        return node;
        //    }

        //    const lastNode = findDeepestNode(flTree);
        //    if (!lastNode) return flTree;

        //    // Step 4: Attach BOM tree as children of the deepest FL node
        //    lastNode.children = bomTree;
        //    lastNode.expanded = true;

        //    return flTree;
        //}

        //function buildMergedTree(flData, flatBOMData) {
        //    // --- Step 1: Build Functional Location Tree ---
        //    const flTree = buildFLTree(flData);

        //    // --- Step 2: Build BOM Tree (Grouped by BOMId) ---
        //    const bomTree = buildTree(flatBOMData);

        //    // --- Step 3: Create a lookup for FL nodes by fullCode ---
        //    const flMap = {};
        //    (function mapFLNodes(nodes) {
        //        nodes.forEach(node => {
        //            flMap[node.fullCode || node.name] = node;
        //            if (node.children && node.children.length) mapFLNodes(node.children);
        //        });
        //    })(flTree);

        //    // --- Step 4: Attach each BOM node under its matching FL node ---
        //    Object.values(bomTree).forEach(bomRoot => attachBOMToFL(bomRoot));

        //    function attachBOMToFL(bomNode) {
        //        const targetFL = bomNode.FL || bomNode.Func_Location;
        //        if (targetFL && flMap[targetFL]) {
        //            const flNode = flMap[targetFL];
        //            flNode.children = flNode.children || [];
        //            flNode.children.push(bomNode);
        //        }

        //        // Recursively attach children
        //        if (bomNode.children && bomNode.children.length) {
        //            bomNode.children.forEach(child => attachBOMToFL(child));
        //        }
        //    }

        //    // --- Step 5: Properly nest subequipments based on FL code hierarchy ---
        //    nestSubEquipmentsInFL(flTree, flMap);

        //    // --- Step 6: Collapse all nodes initially ---
        //    collapseAll(flTree);

        //    // --- Step 7: Assign levels for indentation ---
        //    assignLevels(flTree, 0);

        //    return flTree;

        //    // ---------------- Helper Functions ----------------

        //    function nestSubEquipmentsInFL(tree, flMap) {
        //        Object.values(flMap).forEach(node => {
        //            if (!node.fullCode.includes('.')) return; // Skip primary levels
        //            const parentCode = node.fullCode.substring(0, node.fullCode.lastIndexOf('.'));
        //            const parentNode = flMap[parentCode];
        //            if (parentNode && parentNode !== node) {
        //                // Remove node from any other parent's children first
        //                Object.values(flMap).forEach(p => {
        //                    const idx = p.children?.indexOf(node);
        //                    if (idx !== -1) p.children.splice(idx, 1);
        //                });
        //                // Add under the correct parent
        //                parentNode.children = parentNode.children || [];
        //                parentNode.children.push(node);
        //            }
        //        });
        //    }

        //    function collapseAll(nodes) {
        //        (nodes || []).forEach(node => {
        //            node.expanded = false;
        //            if (node.children && node.children.length)
        //                collapseAll(node.children);
        //        });
        //    }

        //    function assignLevels(nodes, level) {
        //        (nodes || []).forEach(node => {
        //            node.level = level;
        //            if (node.children && node.children.length)
        //                assignLevels(node.children, level + 1);
        //        });
        //    }
        //}

        function buildMergedTree(flData, flatBOMData) {
            // --- Step 1: Build Functional Location Tree ---
            const flTree = buildFLTree(flData);

            // --- Step 2: Build BOM Tree (Grouped by BOMId) ---
            const bomTree = buildTree(flatBOMData);

            // Recursive function to attach BOMs and sort
            function attachBOMs(node) {
                // Match BOMs for this node
                const matchingBOMs = bomTree.filter(b => b.FL === node.fullCode);

                if (matchingBOMs.length > 0) {
                    node.children = node.children.concat(matchingBOMs);
                }

                // Recurse into children
                if (node.children && node.children.length > 0) {
                    node.children.forEach(child => attachBOMs(child));

                    // 🔥 Sort rule: BOMId objects first, then others (FL nodes)
                    node.children.sort((a, b) => {
                        const aIsBOM = !!a.BOMId;
                        const bIsBOM = !!b.BOMId;
                        if (aIsBOM && !bIsBOM) return -1;
                        if (!aIsBOM && bIsBOM) return 1;
                        return 0;
                    });
                }
            }

            // Handle multiple FL roots
            if (Array.isArray(flTree)) {
                flTree.forEach(root => attachBOMs(root));
            } else {
                attachBOMs(flTree);
            }

            return flTree;
        }


        $scope.GenerateFL = function () {
            console.log($scope.fl);

            // Reset each time
            let supLoc = "";
            let funcLoc = "";

            // Superior Location
            supLoc = [
                $scope.fl.Level1 || "",
                $scope.fl.Level2 || "",
                $scope.fl.Level3 || ""
            ].filter(Boolean).join("-");

            if (supLoc) {
                $scope.fl.SuperiorLocation = supLoc;
            }

            console.log($scope.fl.SuperiorLocation);

            // Functional Location
            funcLoc = [
                $scope.fl.Level3 || "",
                $scope.fl.Level4 || "",
                $scope.fl.Level5 || "",
                $scope.fl.Level6 || "",
                $scope.fl.Level7 || "",
                $scope.fl.PrimaryEquipment || "",
                $scope.fl.SubEquipment1 || "",
                $scope.fl.SubEquipment2 || "",
                $scope.fl.SubEquipment3 || ""
            ].filter(Boolean).join("-");

            if (funcLoc) {
                $scope.fl.FunctLocation = funcLoc;
            }

            console.log($scope.fl.FunctLocation);
        };


        $scope.SubmitFL = function () {
            if ($scope.fl.FunctLocation != "") {
                var frmData = new FormData();
                frmData.append("Data", JSON.stringify($scope.fl));
                $http({
                    method: 'POST',
                    url: '/FAR/InsertFuncLoc',
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: frmData,
                }).success(function (response) {
                    console.log(response)
                    if (response === false)
                        $rootScope.Res = "Data already exists";
                    else {
                        $rootScope.Res = "Data created successfully";
                        $scope.BindFL();
                    }

                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.reset();
                }).error(function (err) {
                    console.log(err);
                })
            }
            else {
                $rootScope.Res = "Please generate Functional Location";
                $rootScope.Notify = "alert-danger";
                $rootScope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
            }
        }
        $scope.UpdateFL = function () {
            if ($scope.fl.FunctLocation != "") {
                delete $scope.fl._id;
                var frmData = new FormData();
                frmData.append("Data", JSON.stringify($scope.fl));
                $http({
                    method: 'POST',
                    url: '/FAR/UpdateFuncLoc',
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: frmData,
                }).success(function (response) {
                    console.log(response)
                    if (response === false)
                        $rootScope.Res = "Data failed to update";
                    else {
                        $rootScope.Res = "Data update successfully";
                        $scope.BindFL();
                    }

                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.reset();

                    $scope.btnSubmit = true;
                    $scope.btnUpdate = false;
                }).error(function (err) {
                    console.log(err);
                })
            }
            else {
                $rootScope.Res = "Please generate Functional Location";
                $rootScope.Notify = "alert-danger";
                $rootScope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
            }
        }

        $scope.reset = function () {
            $scope.obj.FARId = "";
            $scope.obj.FARId = "";
            $scope.obj.Region = "";
            $scope.obj.AssetDesc = "";
            $scope.BtnFARmodel = 'Select FAR ID';
            $scope.BtnRegmodel = 'Select Region';
            $scope.form.$setPristine();
            $scope.resetFL();
        }
        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }

        $scope.createData = function () {

            //if (!$scope.form.$invalid) {               

            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = $scope.label;
            var formData = new FormData();
            //$scope.obj.Code = $scope.obj.Code;
            //$scope.obj.Title = $scope.obj.Title;
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/FAR/InsertDataBusiness",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {

                }
                else {
                    if (data === false)
                        $rootScope.Res = "Data already exists";
                    else {
                        $rootScope.Res = "Data created successfully";
                        $scope.BindList();
                    }
                    $scope.reset();
                    $scope.obj = null;

                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }).error(function (data, status, headers, config) {

            });

            // }
        };

        $scope.Edit = function (label, code, title, islive) {
            $scope.btnSubmit = false;
            $scope.btnUpdate = true;
            $scope.obj.Label = label;
            $scope.obj.Code = code;
            $scope.obj.Title = title;
            $scope.obj.Islive = islive;
        }

        $scope.EditFunLoc = function (lst) {
            $scope.btnSubmit = false;
            $scope.btnUpdate = true;
            $scope.fl = lst;
        }

        $scope.DisableFunLoc = function (lst, status) {
            console.log(lst)
            if (confirm("Are you sure, "+ status+" this record?")) {

                $http({
                    method: 'GET',
                    url: '/FAR/DisableFunLoc',
                    params: {
                        section: lst.SectionNo, id: lst.FunctLocation, sts: status == "disable"?false:true
                    }
                }).success(function (response) {
                    $rootScope.Res = $scope.label + " deleted";
                    $rootScope.Notify = "alert-info";
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.BindList();
                    $scope.BindFL();
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        }

        $scope.updateData = function () {
            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = $scope.label;
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));
            $http({
                method: 'POST',
                url: '/FAR/UpdateDataBusiness',
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData,
            }).success(function (response) {
                console.log(response)
                if (response !== false) {
                    $rootScope.Res = "Data updated successfully";
                    $scope.BindFL();
                    $scope.onclickLbl('Section');
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.reset();
                    $scope.obj = {};
                }
            }).error(function (err) {
                console.log(err);
            })
        }

        $rootScope.onclickBusiness = function () {
            //$scope.BindPlantList();
            $scope.BindList();
        }

        $scope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }
        $scope.LoadFileData = function (files) {

            $scope.NotifiyRes = false;
            $scope.$apply();
            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'xls' || angular.lowercase(ext) === 'xlsx') {
                } else {
                    angular.element("input[type='file']").val(null);
                    files[0] = null;

                    $scope.Res = "Load valid excel file";
                    $scope.Notify = "alert-danger";
                    $('#divNotifiy').attr('style', 'display: block');

                    $scope.$apply();



                }
            }
        };
        $scope.ShowHide = false;
        $scope.promise = $scope.MfrBulkUpload = function () {


            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.promise = $http({
                    url: "/FAR/MfrBulkUpload",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    //  alert(data);
                    $scope.ShowHide = false;
                    if (data === 0)
                        $scope.Res = "Records already exists"
                    else $scope.Res = data + " Records uploaded successfully"
                    alert($scope.Res)

                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;

                    //$('#divNotifiy').attr('style', 'display: block');
                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $scope.ShowHide = false;
                    $scope.Res = "Please Valid Your Excel File";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;


                });
            };
        }



        $scope.RemoveMfr = function (id) {

            if (confirm("Are you sure, disable this record?")) {

                $http({
                    method: 'GET',
                    url: '/FAR/RemoveMfr',
                    params: { id: id, IsActive: false, flg: label }
                }).success(function (response) {
                    $rootScope.Res = $scope.label + " deleted";
                    $rootScope.Notify = "alert-info";
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.BindList();
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            } 

        };
        $scope.DisableBuns = function (idx, enable, id) {

            if (enable == false) {
                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/DisableBun',
                        params: { id: id, IsActive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Business disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Business[idx].IsActive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/DisableBun',
                        params: { id: id, IsActive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Business enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Business[idx].IsActive = false;


            }

        };
        $scope.DisableBuns = function (idx, enable, id) {

            if (enable == false) {
                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/DisableNotes',
                        params: { id: id, IsActive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Business disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Business[idx].IsActive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/DisableBun',
                        params: { id: id, IsActive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Business enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Business[idx].IsActive = false;


            }

        };

        $scope.ClearFrm = function () {
            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
            $scope.resetFL();
        }

        $scope.AddMfr = function (term) {
            $scope.mfr = {};
            $scope.mfr.Label = $scope.label;
            var formData = new FormData();
            $scope.mfr.Code = term;
            formData.append("data", angular.toJson($scope.mfr));

            return $http({
                url: "/FAR/InsertMfr",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (response) {
                if (response.includes("successfully")) {
                    $rootScope.Res = response;
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.reset();
                    $scope.obj = null;
                    $scope.mfr = null;
                    $scope.notMfr = false;
                    $scope.BindList();
                }
                else {
                    $rootScope.Res = response;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            });
        }


    });

    app.factory("AutoCompleteService", ["$http", function ($http) {
        return {
            search: function (term) {
                return $http({
                    url: "/Dictionary/AutoCompleteAssetNoun",
                    params: { term: term },
                    method: "GET"
                }).success(function (response) {
                    return response.data;
                });
            }
        };
    }]);
    app.directive("autoComplete", ["AutoCompleteService", function (AutoCompleteService) {
        return {
            restrict: "A",
            link: function (scope, elem, attr, ctrl) {
                elem.autocomplete({
                    source: function (searchTerm, response) {

                        AutoCompleteService.search(searchTerm.term).success(function (autocompleteResults) {

                            response($.map(autocompleteResults, function (autocompleteResult) {
                                return {
                                    label: autocompleteResult.Noun,
                                    value: autocompleteResult
                                }
                            }))
                        });
                    },
                    minLength: 1,
                    select: function (event, selectedItem, http) {
                        scope.NM.Noun = selectedItem.item.value;


                        $.get("/Dictionary/GetNounDetail", { Noun: scope.NM.Noun }).success(function (response) {

                            scope.NM = response;
                            scope.$apply();
                            event.preventDefault();
                        });
                    }
                });

            }

        };
    }]);
    app.factory("AutoCompleteService1", ["$http", function ($http) {
        return {
            search: function (Noun, term) {

                return $http({
                    url: "/Dictionary/AutoCompleteModifier",
                    params: { term: term, Noun: Noun },

                    method: "GET"
                }).success(function (response) {
                    return response.data;
                });


                // return $http.get("AutoCompleteModifier?term=" + term + "&Noun=" + Noun).then(function (response) {
                //  return response.data;
                // });
            }
        };
    }]);
    app.directive("autoComplete1", ["AutoCompleteService1", function (AutoCompleteService) {
        return {
            restrict: "A",
            link: function (scope, elem, attr, ctrl) {
                elem.autocomplete({
                    source: function (searchTerm, response) {

                        AutoCompleteService.search(scope.NM.Noun, searchTerm.term).success(function (autocompleteResults) {
                            response($.map(autocompleteResults, function (autocompleteResult) {
                                return {
                                    label: autocompleteResult.Modifier,
                                    value: autocompleteResult
                                }
                            }))
                        });
                    },
                    minLength: 1,
                    select: function (event, selectedItem) {
                        scope.NM.Modifier = selectedItem.item.value;

                        $.get("/Dictionary/GetNounModifier", { Noun: scope.NM.Noun, Modifier: scope.NM.Modifier }).success(function (response) {
                            scope.NM = response.One_NounModifier;
                            var uomlist = response.One_NounModifier.uomlist;

                            if (uomlist != null) {
                                angular.forEach(scope.UOMs, function (lst) {

                                    if (uomlist.indexOf(lst._id) !== -1) {
                                        lst.Checked = '1';
                                    } else {
                                        lst.Checked = '0';
                                    }
                                });
                            }
                            //angular.forEach(scope.UOMs, function (value1, key) {

                            //    angular.forEach(scope.uomlist, function (value2, key) {


                            //        if (value1._id == value2._id)
                            //        {
                            //            alert("1")
                            //            $('#chku' + _id).Checked = true;

                            //        }

                            //    });


                            //});

                            if (response.ALL_NM_Attributes.length > 0) {
                                scope.rows = response.ALL_NM_Attributes;
                                //  alert(angular.toJson(scope.rows))
                                //angular.forEach(scope.rows, function (lst) {

                                //    $.get("/Dictionary/GetAttributesDetail?Name=" + lst.Characteristic).success(function (response) {
                                //        if (response != null) {
                                //            var i = 0;
                                //            if (response.ValueList == null) {

                                //                angular.forEach($scope.ValueList, function (lst) {
                                //                  $scope.selectValue.push(lst._id);

                                //                });
                                //            } 
                                //        }

                                //    })
                                //});

                                //  alert(angular.toJson(scope.rows));
                            }
                            else {
                                scope.NM.NounDefinition = "";
                                scope.NM.Modifierabv = "";
                                scope.NM.ModifierDefinition = "";
                                scope.rows = [{ 'Squence': 1, 'ShortSquence': 1, 'Remove': 0 }];
                            }
                            scope.$apply();
                            event.preventDefault();
                        });


                        //$.ajax({
                        //      url: 'GetNounModifier?Noun=' + scope.NM.Noun + '&Modifier=' + scope.NM.Modifier,                           
                        //    type: 'GET',                       
                        //    success: function (response) {
                        //       // alert(JSON.stringify(response.ALL_NM_Attributes));
                        //        scope.NM = response.One_NounModifier;
                        //        scope.rows = response.ALL_NM_Attributes;
                        //        scope.$apply();
                        //        event.preventDefault();
                        //    },
                        //    error: function (xhr, ajaxOptions, thrownError) {
                        //        $scope.Res = thrownError;

                        //    }
                        //});
                    }
                });


            }
        };
    }]);
    app.directive('capitalize', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attrs, modelCtrl) {
                var capitalize = function (inputValue) {
                    if (inputValue == undefined) inputValue = '';
                    var capitalized = inputValue.toUpperCase();
                    if (capitalized !== inputValue) {
                        // see where the cursor is before the update so that we can set it back
                        var selection = element[0].selectionStart;
                        modelCtrl.$setViewValue(capitalized);
                        modelCtrl.$render();
                        // set back the cursor after rendering
                        element[0].selectionStart = selection;
                        element[0].selectionEnd = selection;
                    }
                    return capitalized;
                }
                modelCtrl.$parsers.push(capitalize);
                capitalize(scope[attrs.ngModel]); // capitalize initial value
            }
        };
    });

    app.directive('flTree', function ($timeout) {
        return {
            restrict: 'E',
            scope: { nodes: '=' },
            template: `
      <ul class="fl-tree">
        <li ng-repeat="node in nodes" class="fl-node">

          <div class="node-line"
               ng-click="toggle(node, $event)"
               ng-style="{'margin-left': (node.level * 20) + 'px', 'cursor':'pointer'}">

            <!-- Expand/Collapse Icon -->
            <i ng-if="node.children && node.children.length"
               class="fa toggle-icon"
               ng-class="node.expanded ? 'fa-minus-square text-dark' : 'fa-plus-square text-dark'"></i>

            <!-- Node Type Icon -->
            <i class="fa node-icon" ng-class="getNodeIcon(node)"></i>

            <!-- Node Label -->
            <span class="node-label" style="display:flex;width:100%;gap:4px;align-items:center;">
              <b>{{ getLabelPrefix(node) }}</b>
              <span>{{ node.name }}</span>
            </span>
          </div>

          <!-- Recursive child rendering -->
          <fl-tree ng-if="node.expanded && node.children && node.children.length"
                   nodes="node.children"></fl-tree>
        </li>
      </ul>
    `,
            link: function (scope) {

                // --- Collapse all nodes initially ---
                function collapseAll(nodes, level = 0) {
                    (nodes || []).forEach(node => {
                        node.expanded = false;
                        node.level = level;
                        if (node.children && node.children.length)
                            collapseAll(node.children, level + 1);
                    });
                }

                // Wait for data binding
                $timeout(() => {
                    collapseAll(scope.nodes);
                }, 0);

                // --- Toggle expand/collapse ---
                scope.toggle = function (node, event) {
                    event?.stopPropagation?.();
                    if (node.children && node.children.length) {
                        node.expanded = !node.expanded;
                    }
                };

                // --- Node Icons (based on equipment type) ---
                scope.getNodeIcon = function (node) {
                    switch (node.type) {
                        case 'primary': return 'fa-server text-warning'; // Primary Equipment
                        case 'sub1': return 'fa-cogs text-primary';     // Sub Equipment 1
                        case 'sub2': return 'fa-cog text-secondary';    // Sub Equipment 2
                        case 'sub3': return 'fa-wrench text-info';      // Sub Equipment 3
                        default: return 'fa-sitemap text-muted';        // Generic node
                    }
                };

                // --- Label Prefix ---
                scope.getLabelPrefix = function (node) {
                    switch (node.type) {
                        case 'primary': return 'Primary: ';
                        case 'sub1': return 'Sub-Eq1: ';
                        case 'sub2': return 'Sub-Eq2: ';
                        case 'sub3': return 'Sub-Eq3: ';
                        default: return '';
                    }
                };
            }
        };
    });

    app.directive('mergedTree', function ($timeout, $compile) {
        return {
            restrict: 'E',
            scope: { nodes: '=' },
            template: `
      <ul class="bom-tree">
        <li ng-repeat="node in nodes" class="bom-node" style="position:relative;">

          <div class="node-line"
               ng-click="toggle(node, $event)"
               ng-style="{'margin-left': (node.level * 2) + 'px', 'cursor':'pointer'}">

            <!-- Expand/Collapse Icon -->
            <i ng-if="node.children && node.children.length"
               class="fa toggle-icon"
               ng-class="node.expanded ? 'fa-minus-square text-dark' : 'fa-plus-square text-dark'"></i>

            <!-- Node Type Icon -->
            <i class="fa node-icon" ng-class="getNodeIcon(node)"></i>

            <!-- Node Label -->
            <span class="node-label" style="display:flex;width:100%;gap:4px;align-items:center;">
              <b>{{ getLabelPrefix(node) }}</b>
              <span title="{{ node.AssemblyDesc || node.ComponentDesc }}">
                  {{ node.name || node.BOMDesc ||
                     ((node.AssemblyDesc || node.ComponentDesc) | limitTo:20) }}
                  <span ng-if="(node.AssemblyDesc && node.AssemblyDesc.length > 20) ||
                               (node.ComponentDesc && node.ComponentDesc.length > 20)">...</span>
                </span>


              <!-- Quantity + UOM -->
              <span ng-if="node.Quantity" class="qcard">
                {{ node.Quantity }} {{ node.UOM }}
              </span>

              <!-- 🔍 Search Icon (Info button) -->
              <i ng-if="node.Materialcode || node.Description|| node.BOMDesc"
                 class="fa fa-search text-primary"
                 style="cursor:pointer;margin-left:4px;"
                 ng-click="showInfoCard($event, node)">
              </i>
            </span>
          </div>


          <!-- Info Card -->
            <div ng-if="node.showInfo"
                 class="info-card shadow-lg animate-fadein"
                 ng-click="$event.stopPropagation()">

              <div class="info-header">
                <span class="info-title"><i class="fa fa-info-circle"></i>Details</span>
                <i class="fa fa-times close-btn" ng-click="node.showInfo=false"></i>
              </div>

              <div class="info-body">
                <div class="info-section">
                  <div class="info-label">Material Code:</div>
                  <div class="info-value code">{{ node.Materialcode || '-' }}</div>
                </div>

                <div class="info-section">
                  <div class="info-label">Description:</div>
                  <div class="info-value">{{ node.BOMDesc || node.AssemblyDesc|| node.ComponentDesc || '-' }}</div>
                </div>

                <div class="info-section">
                  <div class="info-label">Complete Description:</div>
                  <div class="info-value long-text">{{ node.BOMLongDesc || node.AssemblyLongDesc|| node.ComponentLongDesc || '-' }}</div>
                </div>
              </div>
            </div>


          <!-- Recursive Children -->
          <merged-tree ng-if="node.expanded && node.children && node.children.length"
                       nodes="node.children"></merged-tree>
        </li>
      </ul>
    `,
            link: function (scope) {

                // --- Collapse all nodes initially ---
                function collapseAll(nodes, level = 0) {
                    (nodes || []).forEach(node => {
                        node.expanded = false;
                        node.level = node.level ?? level;
                        if (node.children && node.children.length)
                            collapseAll(node.children, level + 1);
                    });
                }

                $timeout(() => collapseAll(scope.nodes), 0);

                // --- Toggle expand/collapse ---
                scope.toggle = function (node, event) {
                    event?.stopPropagation?.();
                    if (node.children && node.children.length) {
                        node.expanded = !node.expanded;
                    } else if (!node.childrenLoaded && (node.Category === 'I' || node.Category === 'SI')) {
                        node.loading = true;
                        $timeout(function () {
                            node.children = [
                                {
                                    Category: 'SH',
                                    BOMDesc: 'Spare Header',
                                    Quantity: '',
                                    UOM: '',
                                    children: [
                                        { Category: 'E', ComponentDesc: 'Component 1', Quantity: 2, UOM: 'Nos' },
                                        { Category: 'E', ComponentDesc: 'Component 2', Quantity: 4, UOM: 'Nos' }
                                    ]
                                }
                            ];
                            node.childrenLoaded = true;
                            node.loading = false;
                            node.expanded = true;
                        }, 300);
                    }
                };

                // --- 🔍 Show Info Card ---
                scope.showInfoCard = function (event, node) {
                    node.showInfo = false;
                    event.stopPropagation();
                    // Close others first
                    function closeAll(nodes) {
                        (nodes || []).forEach(n => {
                            n.showInfo = false;
                            if (n.children && n.children.length) closeAll(n.children);
                        });
                    }
                    closeAll(scope.nodes);
                    // Open this node
                    node.showInfo = true;
                };

                // --- Icons ---
                scope.getNodeIcon = function (node) {
                    if (node.name && !node.Category)
                        return '';
                    switch (node.Category) {
                        case 'H': return 'fa-sitemap text-warning';
                        case 'I': return 'fa-cogs text-primary';
                        case 'SI': return 'fa-cog text-secondary';
                        case 'SH': return 'fa-layer-group text-info';
                        case 'SSH': return 'fa-indent text-teal';
                        case 'L':
                        case 'T':
                        case 'E': return 'fa-cube text-success';
                        default: return 'fa-circle text-muted';
                    }
                };

                // --- Label Prefix ---
                scope.getLabelPrefix = function (node) {
                    if (node.name && !node.Category)
                        return 'FL: ';

                    switch (node.Category) {
                        case 'H':
                            return node.Tag ? node.Tag + ': ' : 'Equipment: ';

                        case 'I':
                            return node.AssemblyId
                                ? 'ASSEMBLY: ' + node.AssemblyId + ' (' + (node.Materialcode || '-') + ') | '
                                : 'ASSEMBLY: ';

                        case 'SI':
                            return node.AssemblyId
                                ? 'SUB ASSEMBLY: ' + node.AssemblyId + ' (' + (node.Materialcode || '-') + ') | '
                                : 'SUB ASSEMBLY: ';

                        case 'SH':
                            return 'Sub-Equipment: ';

                        case 'SSH':
                            return 'SubSub-Equipment: ';

                        case 'L':
                        case 'T':
                        case 'E':
                            return node.Materialcode
                                ? node.Materialcode + ' | '
                                : '';

                        default:
                            return 'Item: ';
                    }
                };
            }
        };
    });


})();