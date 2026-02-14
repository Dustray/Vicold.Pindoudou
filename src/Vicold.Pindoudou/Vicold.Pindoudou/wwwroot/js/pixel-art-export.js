// 计算颜色的亮度
function calculateColorLuminance(color) {
    // 解析颜色值
    let r, g, b;
    
    if (color.startsWith('#')) {
        color = color.substring(1);
    }
    
    if (color.length === 6) {
        // #RRGGBB 格式
        r = parseInt(color.substring(0, 2), 16);
        g = parseInt(color.substring(2, 4), 16);
        b = parseInt(color.substring(4, 6), 16);
    } else if (color.length === 8) {
        // #RRGGBBAA 格式
        if (color.toLowerCase().endsWith('00')) {
            // #RRGGBBAA 格式
            r = parseInt(color.substring(0, 2), 16);
            g = parseInt(color.substring(2, 4), 16);
            b = parseInt(color.substring(4, 6), 16);
        }
    } else if (color.length === 3) {
        // #RGB 格式
        r = parseInt(color.substring(0, 1) + color.substring(0, 1), 16);
        g = parseInt(color.substring(1, 2) + color.substring(1, 2), 16);
        b = parseInt(color.substring(2, 3) + color.substring(2, 3), 16);
    } else {
        // 默认黑色
        return 0;
    }
    
    // 计算亮度（使用相对亮度公式）
    return (0.299 * r + 0.587 * g + 0.114 * b) / 255;
}

// 判断颜色是否为浅色
function isLightColor(color) {
    // 计算亮度
    const luminance = calculateColorLuminance(color);
    
    // 亮度大于0.5视为浅色
    return luminance > 0.5;
}

// 根据背景颜色获取对比度高的文本颜色（黑色或白色）
function getContrastTextColor(color) {
    // 计算亮度
    const luminance = calculateColorLuminance(color);
    
    // 如果亮度大于0.5，使用黑色文本；否则使用白色文本
    return luminance > 0.5 ? '#000000' : '#FFFFFF';
}

// 全局变量，用于保存缩放状态
window.zoomState = {
    scale: 1,
    translateX: 0,
    translateY: 0
};

// 根据背景颜色获取对比度高的文本颜色（黑色或白色）
function getContrastTextColor(color) {
    // 解析颜色值
    let r, g, b;
    
    if (color.startsWith('#')) {
        color = color.substring(1);
    }
    
    if (color.length === 6) {
        // #RRGGBB 格式
        r = parseInt(color.substring(0, 2), 16);
        g = parseInt(color.substring(2, 4), 16);
        b = parseInt(color.substring(4, 6), 16);
    }
    else if (color.length === 9) {
        // #RRGGBBAA 格式
        if (color.endsWith('00')) {
            // #RRGGBBAA 格式
            r = parseInt(color.substring(0, 2), 16);
            g = parseInt(color.substring(2, 4), 16);
            b = parseInt(color.substring(4, 6), 16);
        }
    }
    else {
        // 默认黑色
        return '#000000';
    }
    
    // 计算亮度（使用相对亮度公式）
    const luminance = (0.299 * r + 0.587 * g + 0.114 * b) / 255;
    
    // 如果亮度大于0.5，使用黑色文本；否则使用白色文本
    return luminance > 0.5 ? '#000000' : '#FFFFFF';
}

// 全局函数，供 Blazor 调用
window.initializeZoomAndPan = function() {
    // 尝试获取元素
    const container = document.getElementById('zoomableContainer');
    const content = document.getElementById('zoomableContent');
    
    if (!container || !content) {
        return;
    }
    
    let isDragging = false;
    let startX = 0;
    let startY = 0;
    
    // 触摸相关变量
    let touchStartDistance = 0;
    let touchStartMidpointX = 0;
    let touchStartMidpointY = 0;
    let isTouchDragging = false;
    let lastDistance = 0;
    
    // 从全局变量恢复缩放状态
    let scale = window.zoomState.scale;
    let translateX = window.zoomState.translateX;
    let translateY = window.zoomState.translateY;
    
    // 初始化位置，使内容居中（仅在首次加载时）
    const centerContent = () => {
        const containerRect = container.getBoundingClientRect();
        const contentRect = content.getBoundingClientRect();
        
        translateX = (containerRect.width - contentRect.width * scale) / 2;
        translateY = (containerRect.height - contentRect.height * scale) / 2;
        
        // 保存初始状态
        window.zoomState.scale = scale;
        window.zoomState.translateX = translateX;
        window.zoomState.translateY = translateY;
        
        updateTransform();
    };
    
    // 更新变换
    const updateTransform = () => {
        const transform = `translate(${translateX}px, ${translateY}px) scale(${scale})`;
        content.style.transform = transform;
        
        // 保存状态
        window.zoomState.scale = scale;
        window.zoomState.translateX = translateX;
        window.zoomState.translateY = translateY;
    };
    
    // 鼠标按下事件
    container.addEventListener('mousedown', (e) => {
        // 左键点击像素元素时，不启动拖动功能
        // 右键点击时，无论是否是像素元素，都启动拖动功能
        const target = e.target;
        const isPixel = target.classList.contains('pixel') || target.closest('.pixel');
        
        if (!isPixel || e.button === 2) {
            isDragging = true;
            startX = e.clientX - translateX;
            startY = e.clientY - translateY;
            container.style.userSelect = 'none';
        }
    });
    
    // 鼠标移动事件
    container.addEventListener('mousemove', (e) => {
        if (!isDragging) return;
        
        translateX = e.clientX - startX;
        translateY = e.clientY - startY;
        updateTransform();
    });
    
    // 鼠标释放事件
    container.addEventListener('mouseup', () => {
        isDragging = false;
        container.style.userSelect = 'auto';
    });
    
    // 鼠标离开事件
    container.addEventListener('mouseleave', () => {
        isDragging = false;
        container.style.userSelect = 'auto';
    });
    
    // 滚轮缩放事件
    container.addEventListener('wheel', (e) => {
        e.preventDefault();
        
        // 计算缩放因子
        const scaleFactor = e.deltaY > 0 ? 0.9 : 1.1;
        const newScale = Math.max(0.1, Math.min(5, scale * scaleFactor));
        
        // 计算鼠标在容器中的位置
        const rect = container.getBoundingClientRect();
        const mouseX = e.clientX - rect.left;
        const mouseY = e.clientY - rect.top;
        
        // 计算缩放前后鼠标在内容上的位置
        const relativeX = (mouseX - translateX) / scale;
        const relativeY = (mouseY - translateY) / scale;
        
        // 计算新的偏移量，使鼠标位置保持不变
        scale = newScale;
        translateX = mouseX - relativeX * scale;
        translateY = mouseY - relativeY * scale;
        
        updateTransform();
    });
    
    // 触摸开始事件
    container.addEventListener('touchstart', (e) => {
        if (e.touches.length === 1) {
            // 单指触摸 - 开始拖动
            isTouchDragging = true;
            startX = e.touches[0].clientX - translateX;
            startY = e.touches[0].clientY - translateY;
            container.style.userSelect = 'none';
        } else if (e.touches.length === 2) {
            // 双指触摸 - 开始缩放
            const touch1 = e.touches[0];
            const touch2 = e.touches[1];
            touchStartDistance = Math.hypot(touch2.clientX - touch1.clientX, touch2.clientY - touch1.clientY);
            lastDistance = touchStartDistance;
            
            container.style.userSelect = 'none';
        }
    });
    
    // 触摸移动事件
    container.addEventListener('touchmove', (e) => {
        if (e.touches.length === 1 && isTouchDragging) {
            // 单指触摸 - 拖动
            e.preventDefault();
            translateX = e.touches[0].clientX - startX;
            translateY = e.touches[0].clientY - startY;
            updateTransform();
        } else if (e.touches.length === 2) {
            // 双指触摸 - 缩放
            e.preventDefault();
            const touch1 = e.touches[0];
            const touch2 = e.touches[1];
            const currentDistance = Math.hypot(touch2.clientX - touch1.clientX, touch2.clientY - touch1.clientY);
            
            if (touchStartDistance > 0 && lastDistance > 0) {
                // 计算缩放因子，基于距离变化率
                const distanceChange = currentDistance - lastDistance;
                const baseZoomSpeed = 0.004; // 基础缩放速度因子
                
                // 基于当前缩放值的自适应缩放速度
                // 当缩放值较小时，减小缩放速度，保持一致的缩放体验
                const adaptiveZoomSpeed = baseZoomSpeed * Math.max(0.5, scale);
                
                // 计算新的缩放值，添加边界限制
                const newScale = Math.max(0.1, Math.min(5, scale + distanceChange * adaptiveZoomSpeed));
                
                // 计算当前双指中心点
                const currentMidpointX = (touch1.clientX + touch2.clientX) / 2;
                const currentMidpointY = (touch1.clientY + touch2.clientY) / 2;
                
                // 计算缩放前后中心点在内容上的位置
                const rect = container.getBoundingClientRect();
                const midpointX = currentMidpointX - rect.left;
                const midpointY = currentMidpointY - rect.top;
                
                const relativeX = (midpointX - translateX) / scale;
                const relativeY = (midpointY - translateY) / scale;
                
                // 计算新的偏移量，使中心点位置保持不变
                scale = newScale;
                translateX = midpointX - relativeX * scale;
                translateY = midpointY - relativeY * scale;
                
                updateTransform();
                lastDistance = currentDistance;
            }
        }
    });
    
    // 触摸结束事件
    container.addEventListener('touchend', (e) => {
        isTouchDragging = false;
        touchStartDistance = 0;
        container.style.userSelect = 'auto';
    });
    
    // 触摸取消事件
    container.addEventListener('touchcancel', (e) => {
        isTouchDragging = false;
        touchStartDistance = 0;
        container.style.userSelect = 'auto';
    });
    
    // 检查是否是首次加载（缩放状态为默认值）
    if (window.zoomState.scale === 1 && window.zoomState.translateX === 0 && window.zoomState.translateY === 0) {
        // 首次加载，居中显示
        centerContent();
    } else {
        // 非首次加载，恢复之前的状态
        updateTransform();
    }
};

// 导出像素图为图片
window.exportPixelArtAsImage = function(width, height, pixelData, paletteColors, showCodes, hideTransparentCodes) {
    // 配置参数
    const pixelSize = 40; // 每个像素的大小，增大一倍以提高清晰度
    const padding = 70; // 留白大小
    const codeFontSize = 16; // 像素中代码字号
    const paletteTitleFontSize = 40; // 调色板标题字号
    const paletteCodeFontSize = 16; // 调色板项中代码字号
    const paletteCountFontSize = 16; // 调色板项中数量字号
    const paletteItemSize = 40; // 调色板项大小
    const gridNumberFontSize = 16; // 网格编号字号
    
    // 统计每个颜色的使用数量
    const colorCount = {};
    for (let i = 0; i < pixelData.length; i++) {
        const color = pixelData[i];
        
        // 检查是否需要跳过透明色
        let shouldCountColor = true;
            // 判断是否为透明色
        if (hideTransparentCodes && color.length === 9 && color.endsWith('00')) {
            shouldCountColor = false;
        }
        
        if (shouldCountColor) {
            if (colorCount[color]) {
                colorCount[color]++;
            } else {
                colorCount[color] = 1;
            }
        }
    }
    
    // 计算Canvas尺寸
    const canvasWidth = width * pixelSize + padding * 2;
    const paletteColorsArray = Object.entries(paletteColors);
    const maxPerRow = Math.floor((canvasWidth - padding * 2) / (paletteItemSize + 10));
    const paletteHeight = Math.ceil(paletteColorsArray.length / maxPerRow) * (paletteItemSize + 25) + padding * 2;
    const canvasHeight = height * pixelSize + padding * 2 + paletteHeight;
    
    // 创建Canvas元素
    const canvas = document.createElement('canvas');
    canvas.width = canvasWidth;
    canvas.height = canvasHeight;
    const ctx = canvas.getContext('2d');
    
    // 填充背景
    ctx.fillStyle = '#ffffff';
    ctx.fillRect(0, 0, canvasWidth, canvasHeight);
    
    // 绘制像素
    const pixelStartX = padding;
    const pixelStartY = padding;
    
    // 绘制网格编号
    ctx.fillStyle = '#333';
    ctx.font = gridNumberFontSize + 'px Arial';
    ctx.textAlign = 'center';
    ctx.textBaseline = 'middle';
    
    // 绘制完整的灰色边框
    const borderSize = 30;
    ctx.fillStyle = '#f0f0f0';
    
    // 顶部边框
    ctx.fillRect(pixelStartX - borderSize, pixelStartY - borderSize, width * pixelSize + borderSize * 2, borderSize);
    
    // 底部边框
    ctx.fillRect(pixelStartX - borderSize, pixelStartY + height * pixelSize, width * pixelSize + borderSize * 2, borderSize);
    
    // 左侧边框
    ctx.fillRect(pixelStartX - borderSize, pixelStartY - borderSize, borderSize, height * pixelSize + borderSize * 2);
    
    // 右侧边框
    ctx.fillRect(pixelStartX + width * pixelSize, pixelStartY - borderSize, borderSize, height * pixelSize + borderSize * 2);
    
    // 绘制顶部编号（X轴，从左到右）
    ctx.fillStyle = '#333';
    for (let x = 0; x < width; x++) {
        const nx = pixelStartX + x * pixelSize + pixelSize / 2;
        const ny = pixelStartY - borderSize / 2;
        ctx.fillText((x+1).toString(), nx, ny);
    }
    
    // 绘制底部编号（X轴，从左到右）
    for (let x = 0; x < width; x++) {
        const nx = pixelStartX + x * pixelSize + pixelSize / 2;
        const ny = pixelStartY + height * pixelSize + borderSize / 2;
        ctx.fillText((x+1).toString(), nx, ny);
    }
    
    // 绘制左侧编号（Y轴，从下到上，左下角为0）
    for (let y = 0; y < height; y++) {
        const nx = pixelStartX - borderSize / 2;
        const ny = pixelStartY + (height - 1 - y) * pixelSize + pixelSize / 2;
        ctx.fillText((y+1).toString(), nx, ny);
    }
    
    // 绘制右侧编号（Y轴，从下到上，左下角为0）
    for (let y = 0; y < height; y++) {
        const nx = pixelStartX + width * pixelSize + borderSize / 2;
        const ny = pixelStartY + (height - 1 - y) * pixelSize + pixelSize / 2;
        ctx.fillText((y+1).toString(), nx, ny);
    }
    
    // 绘制像素
    for (let y = 0; y < height; y++) {
        for (let x = 0; x < width; x++) {
            const index = y * width + x;
            const color = pixelData[index];
            const px = pixelStartX + x * pixelSize;
            const py = pixelStartY + y * pixelSize;
            
            // 绘制像素背景
            ctx.fillStyle = color;
            ctx.fillRect(px, py, pixelSize, pixelSize);
            
            // 绘制像素边框
            ctx.strokeStyle = '#eee';
            ctx.lineWidth = 1;
            ctx.strokeRect(px, py, pixelSize, pixelSize);
            
            // 如果需要显示代码
            if (showCodes) {
                // 检查是否需要隐藏透明色代码
                let shouldShowCode = true;
                if (hideTransparentCodes && color.length === 9 && color.endsWith('00')) {
                    shouldShowCode = false;
                }
                
                if (shouldShowCode) {
                    // 查找对应的代码
                    let code = '';
                    for (const [codeValue,colorKey] of Object.entries(paletteColors)) {
                        if (colorKey === color) {
                            code = codeValue;
                            break;
                        }
                    }
                    
                    // 绘制代码
                    if (code) {
                        // 根据背景颜色计算文本颜色
                        const textColor = getContrastTextColor(color);
                        ctx.fillStyle = textColor;
                        ctx.font = codeFontSize + 'px Arial';
                        ctx.textAlign = 'center';
                        ctx.textBaseline = 'middle';
                        ctx.fillText(code, px + pixelSize / 2, py + pixelSize / 2);
                    }
                }
            }
        }
    }
    
    // 绘制调色板示例
    const paletteStartY = pixelStartY + height * pixelSize + padding;
    const paletteStartX = padding;
    
    // 绘制调色板标题
    ctx.fillStyle = '#333';
    ctx.font = paletteTitleFontSize + 'px Arial';
    ctx.textAlign = 'center';
    ctx.fillText('调色板', canvasWidth / 2, paletteStartY + 20);
    
    // 绘制调色板项
    let paletteX = paletteStartX;
    let paletteY = paletteStartY + 40;
    let count = 0;
    
    for (const [code, color] of paletteColorsArray) {
        // 计算每个调色板项的宽度，使它们从左到右占满
        const colorCountValue = colorCount[color] || 0;
        if (colorCountValue == 0) {
            continue;    
        }
        const itemWidth = (canvasWidth - padding * 2 - (maxPerRow - 1) * 10) / maxPerRow;
        
        // 绘制色块
        ctx.fillStyle = color;
        ctx.fillRect(paletteX, paletteY, itemWidth, paletteItemSize);
        
        // 绘制色块边框
        ctx.strokeStyle = '#eee';
        ctx.lineWidth = 1;
        ctx.strokeRect(paletteX, paletteY, itemWidth, paletteItemSize);
        
        // 绘制代码（放到色块内部）
        // 根据背景颜色的明暗程度自动调整文字颜色
        ctx.fillStyle = isLightColor(color) ? '#333' : '#fff';
        ctx.font = paletteCodeFontSize + 'px Arial';
        ctx.textAlign = 'center';
        ctx.textBaseline = 'middle';
        ctx.fillText(code, paletteX + itemWidth / 2, paletteY + paletteItemSize / 2);
        
        // 绘制数量
        ctx.fillStyle = '#666';
        ctx.font = paletteCountFontSize + 'px Arial';
        ctx.textAlign = 'center';
        ctx.textBaseline = 'top';
        ctx.fillText(colorCountValue, paletteX + itemWidth / 2, paletteY + paletteItemSize + 4);
        
        // 更新位置
        count++;
        if (count % maxPerRow === 0) {
            paletteX = paletteStartX;
            paletteY += paletteItemSize + 40;
        } else {
            paletteX += itemWidth + 10;
        }
    }
    
    // 转换为图片并下载
    const dataURL = canvas.toDataURL('image/png');
    
    // 检查是否支持FileSystemAccess API
    if (window.showSaveFilePicker) {
        // 使用现代的文件保存API
        const options = {
            suggestedName: 'pixel-art.png',
            types: [
                {
                    description: 'PNG 图片文件',
                    accept: {
                        'image/png': ['.png']
                    }
                }
            ]
        };
        
        window.showSaveFilePicker(options).then(async (handle) => {
            // 将dataURL转换为Blob
            const response = await fetch(dataURL);
            const blob = await response.blob();
            
            const writable = await handle.createWritable();
            await writable.write(blob);
            await writable.close();
        }).catch(error => {
            console.error('保存文件失败:', error);
            // 失败时 fallback 到传统方式
            downloadImage(dataURL);
        });
    } else {
        // fallback 到传统的下载方式
        downloadImage(dataURL);
    }
    
    // 传统下载方式
    function downloadImage(dataURL) {
        const link = document.createElement('a');
        link.download = 'pixel-art.png';
        link.href = dataURL;
        link.click();
    }
};

// 保存资源文件
window.saveResourceFile = async function(jsonData) {
    try {
        // 创建Blob对象
        const blob = new Blob([jsonData], { type: 'application/json' });
        
        // 检查是否支持FileSystemAccess API
        if (window.showSaveFilePicker) {
            // 使用现代的文件保存API
            const options = {
                suggestedName: 'pixel-art-resource.json',
                types: [
                    {
                        description: 'JSON 资源文件',
                        accept: {
                            'application/json': ['.json']
                        }
                    }
                ]
            };
            
            const handle = await window.showSaveFilePicker(options);
            const writable = await handle.createWritable();
            await writable.write(blob);
            await writable.close();
        } else {
            //  fallback 到传统的下载方式
            const url = URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;
            link.download = 'pixel-art-resource.json';
            link.click();
            
            // 释放URL对象
            setTimeout(() => {
                URL.revokeObjectURL(url);
            }, 100);
        }
    } catch (error) {
        console.error('保存文件失败:', error);
    }
};

// 打开资源文件
window.openResourceFile = function() {
    return new Promise((resolve) => {
        // 创建文件输入元素
        const input = document.createElement('input');
        input.type = 'file';
        input.accept = '.json';
        
        // 监听文件选择事件
        input.addEventListener('change', (e) => {
            const file = e.target.files[0];
            if (file) {
                const reader = new FileReader();
                reader.onload = (event) => {
                    resolve(event.target.result);
                };
                reader.onerror = () => {
                    resolve('');
                };
                reader.readAsText(file);
            } else {
                resolve('');
            }
        });
        
        // 触发文件选择对话框
        input.click();
    });
};