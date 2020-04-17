import http from '@/config/http';

// 上传图片
export const UploadFile = (data) => http.post('/api/Sys/UploadFile', {
    ...data
});

// 获取二级列表
export const GetClassifications = (data) => http.post('/api/DaRen/GetClassifications', {
    ...data
});

// 获取教育领域列表
export const GetDaRenEdus = (data) => http.post('/api/DaRen/GetDaRenEdus', {
    ...data
});

// 获取职业领域列
export const GetDaRenOccupations = (data) => http.post('/api/DaRen/GetDaRenOccupations', {
    ...data
});

// 添加专业领域
export const CreateMajors = (data) => http.post('/api/DaRen/CreateClassifications', {
    ...data
});

// 获取申请填写数据
export const GetAbovementionedData = (data) => http.post('/api/DaRen/GetAbovementionedData', {
    ...data
});

// 添加或修改自我介绍
export const UpdateSelfIntroduction = (data) => http.post('/api/DaRen/UpdateSelfIntroduction', {
    ...data
});

// 删除职业/教育经历
export const DeleteDaRenOccOrEdu = (data) => http.post('/api/DaRen/DeleteDaRenOccOrEdu', {
    ...data
});

// 添加职业经历
export const CreateDaRenOccupations = (data) => http.post('/api/DaRen/CreateDaRenOccupations', {
    ...data
});

// 修改职业经历
export const UpdateDaRenOccupations = (data) => http.post('/api/DaRen/UpdateDaRenOccupations', {
    ...data
});

// 添加教育经历
export const CreateDaRenEdu = (data) => http.post('/api/DaRen/CreateDaRenEdu', {
    ...data
});

// 修改教育经历
export const UpdateDaRenEdu = (data) => http.post('/api/DaRen/UpdateDaRenEdu', {
    ...data
});

// 修改欢迎语
export const UpdateWelcome = (data) => http.post('/api/DaRen/UpdateWelcome', {
    ...data
});

// 添加修改达人达语
export const UpdateGreetings = (data) => http.post('/api/DaRen/UpdateGreetings', {
    ...data
});

// 添加修改专业资格认证
export const UpdateSpecializedPics = (data) => http.post('/api/DaRen/UpdateSpecializedPics', {
    ...data
});

// 提交审核
export const VerifyDaRen = (data) => http.post('/api/DaRen/VerifyDaRen', {
    ...data
});

// 获取实名认证信息
export const GetUserAuthInfo = (data) => http.post('/api/UserAuth/GetUserAuthInfo', {
    ...data
});

// 上传音频
export const UploadVoice = (data) => http.post('/api/DaRen/UploadVoice', {
    ...data
});

// 删除文件
export const DeleteFile = (data) => http.post('/api/DaRen/DeleteFile', {
    ...data
});

// 新增/修改知识库
export const CreateOrUpdateDaRenKnowledge = (data) => http.post('/api/DaRen/CreateOrUpdateDaRenKnowledge', {
    ...data
});

// 获取知识库详情
export const GetDaRenKnowledgeDetail = (data) => http.post('/api/DaRen/GetDaRenKnowledgeDetail', {
    ...data
});

// 获取知识库详情
export const GetMyKnowledges = (data) => http.post('/api/DaRen/GetMyKnowledges', {
    ...data
});

// 获取知识库详情
export const DeleteDaRenKnowledge = (data) => http.post('/api/DaRen/DeleteDaRenKnowledge', {
    ...data
});

// 上传我的视频
export const CreateVideo = (data) => http.post('/api/DaRen/CreateVideo', {
    ...data
});

// 删除我的视频
export const DeleteVideo = (data) => http.post('/api/DaRen/DeleteVideo', {
    ...data
});

// 获取我的视频
export const GetMyVideo = (data) => http.post('/api/DaRen/GetMyVideo', {
    ...data
});

// 设置倍率保护
export const SetProtectRate = (data) => http.post('/api/DaRen/SetProtectRate', {
    ...data
});

